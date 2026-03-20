namespace PersonManager.Tests
{
    public class VersionBumpTests
    {
        [Fact]
        public void CsprojVersion_ShouldChange_AfterBumpScript()
        {
            // UWAGA: Test nie jest izolowany, wykonuje rzeczywisty skrypt i modyfikuje plik produkcyjny.
            // W idealnej sytuacji należałoby mockować operacje na plikach/skryptach.

            var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            var csprojPath = Path.Combine(repoRoot, "PersonManager", "PersonManager.csproj");
            string? original = null;
            string? originalVersion = null;
            try
            {
                original = File.ReadAllText(csprojPath);
                originalVersion = GetVersion(original);

                // Act
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-File bump-version.ps1",
                    WorkingDirectory = repoRoot,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
                var process = System.Diagnostics.Process.Start(psi);
                if (process == null)
                    throw new InvalidOperationException("Nie udało się uruchomić procesu bump-version.ps1.");
                process.WaitForExit();

                var updated = File.ReadAllText(csprojPath);
                var updatedVersion = GetVersion(updated);

                // Assert
                Assert.NotEqual(originalVersion, updatedVersion);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed: {ex.Message}");
            }
            finally
            {
                // Cleanup: przywrócenie oryginalnej wersji
                if (!string.IsNullOrEmpty(original))
                {
                    File.WriteAllText(csprojPath, original);
                }
            }
        }

        private string GetVersion(string csprojContent)
        {
            var startTag = "<Version>";
            var endTag = "</Version>";
            var start = csprojContent.IndexOf(startTag);
            var end = csprojContent.IndexOf(endTag);
            if (start == -1 || end == -1 || end <= start)
                throw new InvalidOperationException("Tag <Version> nie został znaleziony w pliku csproj.");
            start += startTag.Length;
            var version = csprojContent.Substring(start, end - start).Trim();
            if (string.IsNullOrEmpty(version))
                throw new InvalidOperationException("Nie można odczytać wersji z pliku csproj.");
            return version;
        }
    }
}
