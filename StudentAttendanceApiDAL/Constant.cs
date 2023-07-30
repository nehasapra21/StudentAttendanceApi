namespace StudentAttendanceApiDAL
{
    public class Constant
    {
        public enum Type
        {
            SuperAdmin=1,
            RegionalAdmin=2,
            Teacher=3
        }
        public enum Gender
        {
            FeMale = 1,
            Male = 2,
        }

        public enum ClassStatus
        {
            Active = 1,
            Completed = 2,
            Cancel=3,
            Upcoming=4
        }
    }
}
