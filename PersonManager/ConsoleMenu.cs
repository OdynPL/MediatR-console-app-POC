using MediatR;
using FluentValidation;
using PersonManager.Commands;
using PersonManager.Queries;

namespace PersonManager
{
    public class ConsoleMenu
    {
        private readonly IMediator _mediator;
        public ConsoleMenu(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Wybierz opcję:");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.CreatePerson}. Utwórz osobę");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.GetWeather}. Sprawdź pogodę");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.GetAllPersons}. Pobierz wszystkie osoby");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.GetPersonById}. Pobierz osobę po ID");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.UpdatePerson}. Zaktualizuj osobę");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.DeletePerson}. Usuń osobę");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.CreateProject}. Utwórz projekt");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.CreateCompany}. Utwórz firmę");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.CreateAddress}. Utwórz adres");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.CreatePersonWithAddressAndCompany}. Dodaj osobę z adresem i firmą");
                Console.WriteLine($"{(int)GlobalConfig.MenuOption.AddPersonToProject}. Dodaj osobę do projektu");
                Console.WriteLine($"{GlobalConfig.EscMenuValue} - zakończ program");

                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                var trimmed = input.Trim();
                if (trimmed.Equals(GlobalConfig.EscMenuValue, StringComparison.OrdinalIgnoreCase))
                    break;

                string option = trimmed;
                Console.WriteLine();
                try
                {
                    await HandleOptionAsync(option);
                }
                catch (ValidationException ex)
                {
                    Console.WriteLine("Błędy walidacji:");
                    foreach (var error in ex.Errors)
                        Console.WriteLine($" -- {error.PropertyName}: {error.ErrorMessage}");
                }
                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey(true);
            }
        }

        private async Task HandleOptionAsync(string option)
        {
            switch (option)
            {
                case "1": await CreatePersonAsync(); break;
                case "2": await GetWeatherAsync(); break;
                case "3": await GetAllPersonsAsync(); break;
                case "4": await GetPersonByIdAsync(); break;
                case "5": await UpdatePersonAsync(); break;
                case "6": await DeletePersonAsync(); break;
                case "7": await CreateProjectAsync(); break;
                case "8": await CreateCompanyAsync(); break;
                case "9": await CreateAddressAsync(); break;
                case "10": await CreatePersonWithAddressAndCompanyAsync(); break;
                case "11": await AddPersonToProjectAsync(); break;
                default: Console.WriteLine(GlobalConfig.UnknownOptionError); break;
            }
        }

        private async Task CreatePersonAsync()
        {
            Console.WriteLine("Podaj imię:");
            var name = Console.ReadLine();
            Console.WriteLine("Podaj wiek:");
            var ageStr = Console.ReadLine();
            int.TryParse(ageStr, out var age);
            var result = await _mediator.Send(new CreatePersonCommand(name ?? GlobalConfig.DefaultPersonName, age));
            if (result.Success)
                Console.WriteLine($"Utworzono osobę: {result.Name}, wiek: {result.Age}");
            else
                Console.WriteLine($"Błąd: {result.ErrorMessage}");
        }

        private async Task GetWeatherAsync()
        {
            Console.WriteLine("Podaj miasto:");
            var city = Console.ReadLine();
            var result = await _mediator.Send(new GetWeatherQuery(city ?? "Brak miasta"));
            if (result.Success)
                Console.WriteLine($"Pogoda w {result.City}: {result.Description}");
            else
                Console.WriteLine($"Błąd: {result.ErrorMessage}");
        }

        private async Task GetAllPersonsAsync()
        {
            var result = await _mediator.Send(new GetAllPersonsQuery());
            if (result.Count == 0)
                Console.WriteLine("Brak osób w bazie.");
            else
            {
                Console.WriteLine("Lista osób:");
                foreach (var p in result)
                    Console.WriteLine($"ID: {p.Id}, Imię: {p.Name}, Wiek: {p.Age}");
            }
        }

        private async Task GetPersonByIdAsync()
        {
            var id = ReadIntInput("Podaj ID osoby:");
            if (id == null)
            {
                Console.WriteLine("Nieprawidłowe ID.");
                return;
            }
            var result = await _mediator.Send(new GetPersonByIdQuery(id.Value));
            if (result.Success)
                Console.WriteLine($"ID: {result.Id}, Imię: {result.Name}, Wiek: {result.Age}");
            else
                Console.WriteLine($"Błąd: {result.ErrorMessage}");
        }

        private async Task UpdatePersonAsync()
        {
            var id = ReadIntInput("Podaj ID osoby do aktualizacji:");
            if (id == null)
            {
                Console.WriteLine("Nieprawidłowe ID.");
                return;
            }
            Console.WriteLine("Podaj nowe imię:");
            var name = Console.ReadLine();
            var age = ReadIntInput("Podaj nowy wiek:");
            if (age == null)
            {
                Console.WriteLine("Nieprawidłowy wiek.");
                return;
            }
            var result = await _mediator.Send(new UpdatePersonCommand { Id = id.Value, Name = name ?? "Brak imienia", Age = age.Value });
            if (result.Success)
                Console.WriteLine($"Zaktualizowano osobę: {result.Name}, wiek: {result.Age}");
            else
                Console.WriteLine($"Błąd: {result.ErrorMessage}");
        }

        private async Task DeletePersonAsync()
        {
            var id = ReadIntInput("Podaj ID osoby do usunięcia:");
            if (id == null)
            {
                Console.WriteLine("Nieprawidłowe ID.");
                return;
            }
            var result = await _mediator.Send(new DeletePersonCommand { Id = id.Value });
            if (result)
                Console.WriteLine("Osoba została usunięta.");
            else
                Console.WriteLine("Nie znaleziono osoby lub błąd podczas usuwania.");
        }

        private async Task CreateProjectAsync()
        {
            Console.WriteLine("Podaj tytuł projektu:");
            var projectTitle = Console.ReadLine();
            var memberIds = ReadIntListInput("Podaj ID członków (oddzielone przecinkami):");
            var projectResult = await _mediator.Send(new CreateProjectCommand { Title = projectTitle ?? GlobalConfig.DefaultProjectTitle, MemberIds = memberIds });
            Console.WriteLine(projectResult > 0 ? $"Utworzono projekt o ID: {projectResult}" : "Błąd podczas tworzenia projektu.");
        }

        private async Task CreateCompanyAsync()
        {
            Console.WriteLine("Podaj nazwę firmy:");
            var companyName = Console.ReadLine();
            var companyResult = await _mediator.Send(new CreateCompanyCommand { Name = companyName ?? GlobalConfig.DefaultCompanyName });
            Console.WriteLine(companyResult > 0 ? $"Utworzono firmę o ID: {companyResult}" : "Błąd podczas tworzenia firmy.");
        }

        private async Task CreateAddressAsync()
        {
            Console.WriteLine("Podaj ulicę:");
            var street = Console.ReadLine();
            Console.WriteLine("Podaj miasto:");
            var city = Console.ReadLine();
            Console.WriteLine("Podaj kraj:");
            var country = Console.ReadLine();
            var addressResult = await _mediator.Send(new CreateAddressCommand { Street = street ?? GlobalConfig.DefaultStreet, City = city ?? GlobalConfig.DefaultCity, Country = country ?? GlobalConfig.DefaultCountry });
            Console.WriteLine(addressResult > 0 ? $"Utworzono adres o ID: {addressResult}" : "Błąd podczas tworzenia adresu.");
        }

        private async Task CreatePersonWithAddressAndCompanyAsync()
        {
            Console.WriteLine("Podaj imię:");
            var name = Console.ReadLine();
            var age = ReadIntInput("Podaj wiek:");
            var addressId = ReadIntInput("Podaj ID adresu:");
            var companyId = ReadIntInput("Podaj ID firmy:");
            if (age == null || addressId == null || companyId == null)
            {
                Console.WriteLine("Nieprawidłowe dane wejściowe.");
                return;
            }
            var personResult = await _mediator.Send(new CreatePersonCommand(name ?? GlobalConfig.DefaultPersonName, age.Value, addressId.Value, companyId.Value));
            Console.WriteLine(personResult.Success ? $"Utworzono osobę: {personResult.Name}, wiek: {personResult.Age}, adres ID: {addressId}, firma ID: {companyId}" : $"Błąd: {personResult.ErrorMessage}");
        }

        private async Task AddPersonToProjectAsync()
        {
            var personId = ReadIntInput("Podaj ID osoby:");
            var projectId = ReadIntInput("Podaj ID projektu:");
            if (personId == null || projectId == null)
            {
                Console.WriteLine("Nieprawidłowe dane wejściowe.");
                return;
            }
            var addToProjectResult = await _mediator.Send(new AddPersonToProjectCommand { PersonId = personId.Value, ProjectId = projectId.Value });
            Console.WriteLine(addToProjectResult ? "Osoba dodana do projektu." : "Błąd podczas dodawania osoby do projektu.");
        }
        // Helper methods for input validation
        private int? ReadIntInput(string prompt)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            if (int.TryParse(input, out var value))
                return value;
            return null;
        }

        private List<int> ReadIntListInput(string prompt)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                return new List<int>();
            return input.Split(',')
                .Select(s => int.TryParse(s.Trim(), out var mid) ? mid : 0)
                .Where(mid => mid > 0)
                .ToList();
        }
    }
}
