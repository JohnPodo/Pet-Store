namespace PetMeUp.Models
{
    public enum Role
    {
        Admin = 1 ,Employee=2 , User =3
    }

    public enum Severity
    {
        Information = 1, Warning = 2, Error = 3, Exception = 4
    }

    public enum DatabaseType
    {
        MSSQL = 1, SQLite = 2
    }
}