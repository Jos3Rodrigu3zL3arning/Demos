using System.ComponentModel;

namespace ACME.PayrollManagement.Domain.Enums
{
    public enum DayAbbreviationEnum
    {
        [Description("Monday")]
        MO = 1,

        [Description("Tuesday")]
        TU = 2,

        [Description("Wednesday")]
        WE = 3,

        [Description("Thursday")]
        TH = 4,

        [Description("Friday")]
        FR = 5,

        [Description("Saturday")]
        SA = 6,

        [Description("Sunday")]
        SU = 7,
    }
}