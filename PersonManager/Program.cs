using MediatR;
using PersonManager.Commands;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using PersonManager.Queries;

namespace PersonManager
{
    public class Program
    {


        public static async Task Main(string[] args)
        {
            var provider = App.BuildDiContainer();
            App.EnsureAndSeedDatabase(provider);
            var mediator = provider.GetRequiredService<IMediator>();

            while (true)
            {
                var cts = new CancellationTokenSource();
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

                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                    break;

                string option = key.KeyChar.ToString();

                Console.WriteLine();

                try
                {
                    switch (option)
                    {
                        case "1":
                            Console.WriteLine("Podaj imię:");
                            var name = Console.ReadLine();
                            Console.WriteLine("Podaj wiek:");
                            var ageStr = Console.ReadLine();
                            int.TryParse(ageStr, out var age);
                            var result = await mediator.Send(new CreatePersonCommand(name ?? "Brak imienia", age), cts.Token);
                            if (result.Success)
                                Console.WriteLine($"Utworzono osobę: {result.Name}, wiek: {result.Age}");
                            else
                                Console.WriteLine($"Błąd: {result.ErrorMessage}");
                            break;
                        case "2":
                            Console.WriteLine("Podaj miasto:");
                            var city = Console.ReadLine();
                            var result2 = await mediator.Send(new GetWeatherQuery(city ?? "Brak miasta"), cts.Token);
                            if (result2.Success)
                                Console.WriteLine($"Pogoda w {result2.City}: {result2.Description}");
                            else
                                Console.WriteLine($"Błąd: {result2.ErrorMessage}");
                            break;
                        case "3":
                            var result3 = await mediator.Send(new GetAllPersonsQuery(), cts.Token);
                            if (result3.Count == 0)
                                Console.WriteLine("Brak osób w bazie.");
                            else
                            {
                                Console.WriteLine("Lista osób:");
                                foreach (var p in result3)
                                    Console.WriteLine($"ID: {p.Id}, Imię: {p.Name}, Wiek: {p.Age}");
                            }
                            break;
                        case "4":
                            Console.WriteLine("Podaj ID osoby:");
                            var idStr = Console.ReadLine();
                            if (int.TryParse(idStr, out var id))
                            {
                                var result4 = await mediator.Send(new GetPersonByIdQuery(id), cts.Token);
                                if (result4.Success)
                                    Console.WriteLine($"ID: {result4.Id}, Imię: {result4.Name}, Wiek: {result4.Age}");
                                else
                                    Console.WriteLine($"Błąd: {result4.ErrorMessage}");
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowe ID.");
                            }
                            break;
                        case "5":
                            Console.WriteLine("Podaj ID osoby do aktualizacji:");
                            var idStr5 = Console.ReadLine();
                            if (int.TryParse(idStr5, out var id5))
                            {
                                Console.WriteLine("Podaj nowe imię:");
                                var name5 = Console.ReadLine();
                                Console.WriteLine("Podaj nowy wiek:");
                                var ageStr5 = Console.ReadLine();
                                if (int.TryParse(ageStr5, out var age5))
                                {
                                    var result5 = await mediator.Send(new UpdatePersonCommand { Id = id5, Name = name5 ?? "Brak imienia", Age = age5 }, cts.Token);
                                    if (result5.Success)
                                        Console.WriteLine($"Zaktualizowano osobę: {result5.Name}, wiek: {result5.Age}");
                                    else
                                        Console.WriteLine($"Błąd: {result5.ErrorMessage}");
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
                            break;
                        case "6":
                            Console.WriteLine("Podaj ID osoby do usunięcia:");
                            var idStr6 = Console.ReadLine();
                            if (int.TryParse(idStr6, out var id6))
                            {
                                var result6 = await mediator.Send(new DeletePersonCommand { Id = id6 }, cts.Token);
                                if (result6)
                                    Console.WriteLine("Osoba została usunięta.");
                                else
                                    Console.WriteLine("Nie znaleziono osoby lub błąd podczas usuwania.");
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowe ID.");
                            }
                            break;
                        case "7":
                            Console.WriteLine("Podaj tytuł projektu:");
                            var projectTitle = Console.ReadLine();
                            Console.WriteLine("Podaj ID członków (oddzielone przecinkami):");
                            var memberIdsStr = Console.ReadLine();
                            var memberIds = memberIdsStr?.Split(',').Select(s => int.TryParse(s.Trim(), out var mid) ? mid : 0).Where(mid => mid > 0).ToList() ?? new List<int>();
                            var projectResult = await mediator.Send(new CreateProjectCommand { Title = projectTitle ?? "Brak tytułu", MemberIds = memberIds }, cts.Token);
                            Console.WriteLine(projectResult > 0 ? $"Utworzono projekt o ID: {projectResult}" : "Błąd podczas tworzenia projektu.");
                            break;
                        case "8":
                            Console.WriteLine("Podaj nazwę firmy:");
                            var companyName = Console.ReadLine();
                            var companyResult = await mediator.Send(new CreateCompanyCommand { Name = companyName ?? "Brak nazwy" }, cts.Token);
                            Console.WriteLine(companyResult > 0 ? $"Utworzono firmę o ID: {companyResult}" : "Błąd podczas tworzenia firmy.");
                            break;
                        case "9":
                            Console.WriteLine("Podaj ulicę:");
                            var street = Console.ReadLine();
                            Console.WriteLine("Podaj miasto:");
                            var city9 = Console.ReadLine();
                            Console.WriteLine("Podaj kraj:");
                            var country = Console.ReadLine();
                            var addressResult = await mediator.Send(new CreateAddressCommand { Street = street ?? "Brak ulicy", City = city9 ?? "Brak miasta", Country = country ?? "Brak kraju" }, cts.Token);
                            Console.WriteLine(addressResult > 0 ? $"Utworzono adres o ID: {addressResult}" : "Błąd podczas tworzenia adresu.");
                            break;
                        case "10":
                            Console.WriteLine("Podaj imię:");
                            var name10 = Console.ReadLine();
                            Console.WriteLine("Podaj wiek:");
                            var ageStr10 = Console.ReadLine();
                            int.TryParse(ageStr10, out var age10);
                            Console.WriteLine("Podaj ID adresu:");
                            var addressIdStr = Console.ReadLine();
                            int.TryParse(addressIdStr, out var addressId);
                            Console.WriteLine("Podaj ID firmy:");
                            var companyIdStr = Console.ReadLine();
                            int.TryParse(companyIdStr, out var companyId);
                            var personResult10 = await mediator.Send(new CreatePersonCommand(name10 ?? "Brak imienia", age10, addressId, companyId), cts.Token);
                            Console.WriteLine(personResult10.Success ? $"Utworzono osobę: {personResult10.Name}, wiek: {personResult10.Age}, adres ID: {addressId}, firma ID: {companyId}" : $"Błąd: {personResult10.ErrorMessage}");
                            break;
                        case "11":
                            Console.WriteLine("Podaj ID osoby:");
                            var personIdStr = Console.ReadLine();
                            int.TryParse(personIdStr, out var personId);
                            Console.WriteLine("Podaj ID projektu:");
                            var projectIdStr = Console.ReadLine();
                            int.TryParse(projectIdStr, out var projectId);
                            var addToProjectResult = await mediator.Send(new AddPersonToProjectCommand { PersonId = personId, ProjectId = projectId }, cts.Token);
                            Console.WriteLine(addToProjectResult ? "Osoba dodana do projektu." : "Błąd podczas dodawania osoby do projektu.");
                            break;
                        default:
                            Console.WriteLine("Nieznana opcja.");
                            break;
                    }
                }
                catch (ValidationException ex)
                {
                    cts.Cancel();
                    Console.WriteLine("Błędy walidacji:");
                    foreach (var error in ex.Errors)
                        Console.WriteLine($" -- {error.PropertyName}: {error.ErrorMessage}");
                }

                Console.WriteLine("Naciśnij dowolny klawisz, aby kontynuować...");
                Console.ReadKey(true);
            }
        }
    }
}
