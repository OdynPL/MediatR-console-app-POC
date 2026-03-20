namespace PersonManager
{
    public static class GlobalConfig
    {
        // Stałe tekstowe
        public const string DefaultPersonName = "Brak imienia";
        public const string DefaultProjectTitle = "Brak tytułu";
        public const string DefaultCompanyName = "Brak nazwy";
        public const string DefaultStreet = "Brak ulicy";
        public const string DefaultCity = "Brak miasta";
        public const string DefaultCountry = "Brak kraju";

        // Stałe komunikaty błędów
        public const string PersonNameEmptyError = "Imię nie może być puste.";
        public const string PersonAgePositiveError = "Wiek musi być dodatni.";
        public const string IdPositiveError = "ID musi być dodatnie.";
        public const string PersonNameMinLengthError = "Imię musi mieć co najmniej 2 znaki.";
        public const string CityEmptyError = "Miasto nie może być puste.";
        public const string CityMinLengthError = "Miasto musi mieć co najmniej 2 znaki.";
        public const string UnknownOptionError = "Nieznana opcja.";
        public const string EscMenuValue = "ESC";


        // Stała ścieżka do logów (w tym samym folderze co baza app.db)
        public const string LogFilePath = "app.log";

        // Magiczne liczby dla seederów i testów
        public const int AddressCount = 50;
        public const int CompanyCount = 20;
        public const int ProjectCount = 300;
        public const int PersonCount = 1250;
        public const int MinPersonProjects = 2;
        public const int MaxPersonProjects = 8;

        // Przykładowe magiczne liczby
        public const int MinPersonAge = 1;
        public const int MaxPersonAge = 120;

        // Enum dla opcji menu
        public enum MenuOption
        {
            CreatePerson = 1,
            GetWeather = 2,
            GetAllPersons = 3,
            GetPersonById = 4,
            UpdatePerson = 5,
            DeletePerson = 6,
            CreateProject = 7,
            CreateCompany = 8,
            CreateAddress = 9,
            CreatePersonWithAddressAndCompany = 10,
            AddPersonToProject = 11,
            Exit = 99
        }
    }
}
