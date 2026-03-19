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
                Console.WriteLine("1. Utwórz osobę");
                Console.WriteLine("2. Sprawdź pogodę");
                Console.WriteLine("3. Pobierz wszystkie osoby");
                Console.WriteLine("4. Pobierz osobę po ID");
                Console.WriteLine("5. Zaktualizuj osobę");
                Console.WriteLine("6. Usuń osobę");
                Console.WriteLine("7. Utwórz projekt");
                Console.WriteLine("8. Utwórz firmę");
                Console.WriteLine("9. Utwórz adres");
                Console.WriteLine("10. Dodaj osobę z adresem i firmą");
                Console.WriteLine("11. Dodaj osobę do projektu");
                Console.WriteLine("ESC - zakończ program");

                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    continue;
                var trimmed = input.Trim();
                if (trimmed.Equals("ESC", StringComparison.OrdinalIgnoreCase))
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
                default: Console.WriteLine("Nieznana opcja."); break;
            }
        }

        private async Task CreatePersonAsync()
        {
            Console.WriteLine("Podaj imię:");
            var name = Console.ReadLine();
            Console.WriteLine("Podaj wiek:");
            var ageStr = Console.ReadLine();
            int.TryParse(ageStr, out var age);
            var result = await _mediator.Send(new CreatePersonCommand(name ?? "Brak imienia", age));
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
            Console.WriteLine("Podaj ID osoby:");
            var idStr = Console.ReadLine();
            if (int.TryParse(idStr, out var id))
            {
                var result = await _mediator.Send(new GetPersonByIdQuery(id));
                if (result.Success)
                    Console.WriteLine($"ID: {result.Id}, Imię: {result.Name}, Wiek: {result.Age}");
                else
                    Console.WriteLine($"Błąd: {result.ErrorMessage}");
            }
            else
            {
                Console.WriteLine("Nieprawidłowe ID.");
            }
        }

        private async Task UpdatePersonAsync()
        {
            Console.WriteLine("Podaj ID osoby do aktualizacji:");
            var idStr = Console.ReadLine();
            if (int.TryParse(idStr, out var id))
            {
                Console.WriteLine("Podaj nowe imię:");
                var name = Console.ReadLine();
                Console.WriteLine("Podaj nowy wiek:");
                var ageStr = Console.ReadLine();
                if (int.TryParse(ageStr, out var age))
                {
                    var result = await _mediator.Send(new UpdatePersonCommand { Id = id, Name = name ?? "Brak imienia", Age = age });
                    if (result.Success)
                        Console.WriteLine($"Zaktualizowano osobę: {result.Name}, wiek: {result.Age}");
                    else
                        Console.WriteLine($"Błąd: {result.ErrorMessage}");
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wiek.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowe ID.");
            }
        }

        private async Task DeletePersonAsync()
        {
            Console.WriteLine("Podaj ID osoby do usunięcia:");
            var idStr = Console.ReadLine();
            if (int.TryParse(idStr, out var id))
            {
                var result = await _mediator.Send(new DeletePersonCommand { Id = id });
                if (result)
                    Console.WriteLine("Osoba została usunięta.");
                else
                    Console.WriteLine("Nie znaleziono osoby lub błąd podczas usuwania.");
            }
            else
            {
                Console.WriteLine("Nieprawidłowe ID.");
            }
        }

        private async Task CreateProjectAsync()
        {
            Console.WriteLine("Podaj tytuł projektu:");
            var projectTitle = Console.ReadLine();
            Console.WriteLine("Podaj ID członków (oddzielone przecinkami):");
            var memberIdsStr = Console.ReadLine();
            var memberIds = memberIdsStr?.Split(',').Select(s => int.TryParse(s.Trim(), out var mid) ? mid : 0).Where(mid => mid > 0).ToList() ?? new List<int>();
            var projectResult = await _mediator.Send(new CreateProjectCommand { Title = projectTitle ?? "Brak tytułu", MemberIds = memberIds });
            Console.WriteLine(projectResult > 0 ? $"Utworzono projekt o ID: {projectResult}" : "Błąd podczas tworzenia projektu.");
        }

        private async Task CreateCompanyAsync()
        {
            Console.WriteLine("Podaj nazwę firmy:");
            var companyName = Console.ReadLine();
            var companyResult = await _mediator.Send(new CreateCompanyCommand { Name = companyName ?? "Brak nazwy" });
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
            var addressResult = await _mediator.Send(new CreateAddressCommand { Street = street ?? "Brak ulicy", City = city ?? "Brak miasta", Country = country ?? "Brak kraju" });
            Console.WriteLine(addressResult > 0 ? $"Utworzono adres o ID: {addressResult}" : "Błąd podczas tworzenia adresu.");
        }

        private async Task CreatePersonWithAddressAndCompanyAsync()
        {
            Console.WriteLine("Podaj imię:");
            var name = Console.ReadLine();
            Console.WriteLine("Podaj wiek:");
            var ageStr = Console.ReadLine();
            int.TryParse(ageStr, out var age);
            Console.WriteLine("Podaj ID adresu:");
            var addressIdStr = Console.ReadLine();
            int.TryParse(addressIdStr, out var addressId);
            Console.WriteLine("Podaj ID firmy:");
            var companyIdStr = Console.ReadLine();
            int.TryParse(companyIdStr, out var companyId);
            var personResult = await _mediator.Send(new CreatePersonCommand(name ?? "Brak imienia", age, addressId, companyId));
            Console.WriteLine(personResult.Success ? $"Utworzono osobę: {personResult.Name}, wiek: {personResult.Age}, adres ID: {addressId}, firma ID: {companyId}" : $"Błąd: {personResult.ErrorMessage}");
        }

        private async Task AddPersonToProjectAsync()
        {
            Console.WriteLine("Podaj ID osoby:");
            var personIdStr = Console.ReadLine();
            int.TryParse(personIdStr, out var personId);
            Console.WriteLine("Podaj ID projektu:");
            var projectIdStr = Console.ReadLine();
            int.TryParse(projectIdStr, out var projectId);
            var addToProjectResult = await _mediator.Send(new AddPersonToProjectCommand { PersonId = personId, ProjectId = projectId });
            Console.WriteLine(addToProjectResult ? "Osoba dodana do projektu." : "Błąd podczas dodawania osoby do projektu.");
        }
    }
}
