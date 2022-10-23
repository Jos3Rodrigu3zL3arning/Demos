using ACME.PayrollManagement.Application.Constants;
using ACME.PayrollManagement.Domain.DTOs;
using ACME.PayrollManagement.Domain.Enums;
using System.Text;
using System.Text.Json;

namespace ACME.PayrollManagement.Application.Managers.Implementation
{
    public class PaymentManager : IPaymentManager
    {
        #region [ PUBLIC ]

        public async Task<ProcessedFileResponseDTO> ProcessPaymentFilePerWorkerAsync(string filePath, FileSettingsDTO fileSettings)
        {
            var response = new ProcessedFileResponseDTO { Success = false };

            try
            {
                string fileContent = await GetFileContentAsync(filePath, FileConstants.MESSAGE_FOR_NON_EXISTENT_FILE_DIRECTORY);
                ICollection<TimeRangesPerEmployeeDTO> timesPerEmployee = new HashSet<TimeRangesPerEmployeeDTO>();
                string fileContentErrors = ParseFileContent(fileSettings, fileContent, timesPerEmployee);
                bool fileHasContentErrors = !string.IsNullOrEmpty(fileContentErrors);

                if (fileHasContentErrors)
                {
                    response.Error = $"{FileConstants.MESSAGE_FOR_INVALID_FILE_CONTENT} {fileContentErrors}";
                    return response;
                }

                string configurationFileContent = await GetFileContentAsync(fileSettings.ConfigurationFilesPath, FileConstants.MESSAGE_FOR_CONFIGURATION_FILE_NOT_FOUND);
                IEnumerable<ReferenceValueByTimeRange>? referenceValuesByTimeRange = JsonSerializer.Deserialize<IEnumerable<ReferenceValueByTimeRange>>(configurationFileContent);

                if (referenceValuesByTimeRange?.Any() != true)
                {
                    response.Error = FileConstants.ERROR_MESSAGE_FOR_INVALID_PAYMENT_SETTINS;
                    return response;
                }

                response.PaymentDetails = CalculatePaymentsPerEmployee(timesPerEmployee, referenceValuesByTimeRange);
                response.Success = true;
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }

        #endregion [ PUBLIC ]

        #region [ PRIVATE ]

        private static async Task<string> GetFileContentAsync(string filePath, string errorMessageForNotExistingFile)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new ApplicationException(errorMessageForNotExistingFile);
            }

            string fileContent = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrEmpty(fileContent))
            {
                throw new ApplicationException(FileConstants.MESSAGE_FOR_EMPTY_FILE);
            }

            return fileContent;
        }

        private string ParseFileContent(FileSettingsDTO fileSettings, string fileContent, ICollection<TimeRangesPerEmployeeDTO> timesPerEmployee)
        {
            string[] fileLines = fileContent.Split(fileSettings.LineSeparator);
            var fileContentErrors = new StringBuilder();
            int lineCounter = 0;

            foreach (string fileLine in fileLines)
            {
                lineCounter++;
                bool validLineContent = ValidateLineContent(fileSettings, fileLine);

                if (!validLineContent)
                {
                    fileContentErrors.Append("Line ").Append(lineCounter).Append(" of the file does not have valid separators. ");
                    break;
                }

                string employeeName = fileLine.Split(fileSettings.EmployeeNameSeparator)[0].Trim();
                validLineContent = LineContentIsValid(employeeName);

                if (!validLineContent)
                {
                    fileContentErrors.Append("Line ").Append(lineCounter).Append(" doesn't have a valid employee name. ");
                    break;
                }

                string[] fullTimeRanges = fileLine
                    .Replace(employeeName + fileSettings.EmployeeNameSeparator, "")
                    .Split(fileSettings.TimeRangeSeparator);
                validLineContent = LineContentIsValid(fullTimeRanges);

                if (!validLineContent)
                {
                    fileContentErrors.Append("Line ").Append(lineCounter).AppendLine(" has no valid time ranges. ");
                    break;
                }

                bool validTimeRangesPerLine = false;
                foreach (string timeRange in fullTimeRanges)
                {
                    var timeRangesPerEmployee = new TimeRangesPerEmployeeDTO(employeeName);
                    Tuple<bool, DayAbbreviationEnum> validationResult = LineContentIsValid(fileSettings, timeRange);

                    if (!validationResult.Item1)
                    {
                        fileContentErrors.Append("Line '").Append(lineCounter).Append("' has no valid day identifiers. ");
                        break;
                    }

                    timeRangesPerEmployee.DayAbbreviation = validationResult.Item2;

                    string[] startAndEndTimeRanges = timeRange
                        .Replace(timeRangesPerEmployee.DayAbbreviation.ToString(), "")
                        .Split(fileSettings.HourSeparator);
                    Tuple<string, TimeOnly, TimeOnly> resultValidationRangeOfHours = LineContentIsValid(startAndEndTimeRanges, FileConstants.NUMBER_OF_HOURS_ALLOWED_PER_DAY);

                    if (!string.IsNullOrEmpty(resultValidationRangeOfHours.Item1))
                    {
                        fileContentErrors
                            .Append("Line ")
                            .Append(lineCounter)
                            .Append(" has no valid time ranges. ")
                            .Append(resultValidationRangeOfHours.Item1);
                        break;
                    }

                    timeRangesPerEmployee.StartTime = resultValidationRangeOfHours.Item2;
                    timeRangesPerEmployee.EndTime = resultValidationRangeOfHours.Item3;
                    timesPerEmployee.Add(timeRangesPerEmployee);
                    validTimeRangesPerLine = true;
                }

                if (!validTimeRangesPerLine)
                {
                    break;
                }
            }

            return fileContentErrors.ToString();
        }

        private bool ValidateLineContent(FileSettingsDTO fileSettings, string fileLine)
        {
            bool lineContainsValidEmployeeSeparator = fileLine.Contains(fileSettings.EmployeeNameSeparator);
            bool lineContainsValidHourSeparator = fileLine.Contains(fileSettings.HourSeparator);
            bool validLineContent = lineContainsValidEmployeeSeparator
                && lineContainsValidHourSeparator;

            return validLineContent;
        }

        private bool LineContentIsValid(string value) => !string.IsNullOrEmpty(value);

        private bool LineContentIsValid(string[] values) => values?.Any() != true || values?.All(x => !string.IsNullOrEmpty(x)) == true;

        private Tuple<bool, DayAbbreviationEnum> LineContentIsValid(FileSettingsDTO fileSettings, string timeRange)
        {
            string dayIdentifier = timeRange.Substring(fileSettings.DayIdentifierStartIndex, fileSettings.DayIdentifierLength).Trim();
            string? abbreviatedDay = FileConstants.ALLOWED_DAY_ABBREVIATIONS.FirstOrDefault(x => string.Equals(x, dayIdentifier, StringComparison.OrdinalIgnoreCase));
            DayAbbreviationEnum dayAbbreviation = DayAbbreviationEnum.MO;
            bool validDayIdentifier = !string.IsNullOrEmpty(abbreviatedDay) && Enum.TryParse(dayIdentifier, out dayAbbreviation);

            if (!validDayIdentifier)
            {
                return new Tuple<bool, DayAbbreviationEnum>(false, default);
            }

            return new Tuple<bool, DayAbbreviationEnum>(true, dayAbbreviation);
        }

        private Tuple<string, TimeOnly, TimeOnly> LineContentIsValid(string[] values, int numberOfHoursAllowed)
        {
            bool timeRangesComplete = values?.Length > 0 && values.All(x => !string.IsNullOrEmpty(x));
            if (!timeRangesComplete)
            {
                return Tuple.Create(FileConstants.MESSAGE_FOR_HOUR_RANGE_NOT_VALID, (TimeOnly)default, (TimeOnly)default);
            }

            if (values?.Length != numberOfHoursAllowed)
            {
                return Tuple.Create(FileConstants.MESSAGE_FOR_WRONG_NUMBER_OF_TIME_RANGES_PER_DAY, (TimeOnly)default, (TimeOnly)default);
            }

            if (!TimeOnly.TryParse(values[0], out TimeOnly startTime))
            {
                return Tuple.Create(FileConstants.MESSAGE_FOR_INVALID_START_TIME, (TimeOnly)default, (TimeOnly)default);
            }

            if (!TimeOnly.TryParse(values[1], out TimeOnly endTime))
            {
                return Tuple.Create(FileConstants.MESSAGE_FOR_INVALID_END_TIME, (TimeOnly)default, (TimeOnly)default);
            }

            double minutesDifference = (endTime - startTime).TotalMinutes;

            if (startTime >= endTime)
            {
                return Tuple.Create(FileConstants.MESSAGE_FOR_START_TIME_GREATER_THAN_END_TIME, (TimeOnly)default, (TimeOnly)default);
            }

            IEnumerable<(TimeOnly, TimeOnly)> timeRanges = FileConstants.ALLOWED_TIME_RANGES.Where(x => startTime >= x.Item1 && endTime <= x.Item2);

            if (timeRanges?.Any() != true)
            {
                string allowedTimeRanges = string.Join(" - ", FileConstants.ALLOWED_TIME_RANGES.Select(x => $"[{x.Item1} - {x.Item2}]"));
                return Tuple.Create($"{FileConstants.MESSAGE_FOR_INVALID_TIME_RANGE} {allowedTimeRanges}", (TimeOnly)default, (TimeOnly)default);
            }

            return Tuple.Create(string.Empty, startTime, endTime);
        }

        private IEnumerable<PaymentPerEmployeeDTO> CalculatePaymentsPerEmployee(ICollection<TimeRangesPerEmployeeDTO> timesPerEmployee, IEnumerable<ReferenceValueByTimeRange>? referenceValuesByTimeRange)
        {
            IEnumerable<PaymentPerEmployeeDTO> paymentDetails = new HashSet<PaymentPerEmployeeDTO>();
            foreach (TimeRangesPerEmployeeDTO employeeTimes in timesPerEmployee)
            {
                ReferenceValueByTimeRange? referenceValue = referenceValuesByTimeRange!.FirstOrDefault(x => x.Day == employeeTimes.DayAbbreviation
                    && employeeTimes.StartTime >= x.StartTime && employeeTimes.EndTime <= x.EndTime);

                decimal valueToPayPerHour = referenceValue?.ValueToPayPerHour ?? 0;
                employeeTimes.AmountToPayPerTimeRange = employeeTimes.DifferenceInMinutes * (double)valueToPayPerHour / FileConstants.MINUTES_PER_HOUR;
                employeeTimes.CurrencyType = referenceValue?.CurrencyType ?? CurrencyTypeEnum.US;
            }

            paymentDetails = timesPerEmployee
                .GroupBy(x => x.EmployeeName)
                .Select(x => new PaymentPerEmployeeDTO
                {
                    EmployeeName = x.Key,
                    TotalToPay = x.Sum(pay => pay.AmountToPayPerTimeRange),
                    CurrencyType = x.First().CurrencyType.ToString(),
                });

            return paymentDetails;
        }

        #endregion [ PRIVATE ]
    }
}