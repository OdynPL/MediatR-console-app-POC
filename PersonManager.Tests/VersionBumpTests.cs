namespace PersonManager.Tests
{
    public class VersionBumpTests
    {
        [Fact]
        public void CsprojVersion_ShouldChange_AfterBumpScript()
        {
            // Arrange
            var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            var csprojPath = Path.Combine(repoRoot, "PersonManager", "PersonManager.csproj");
            var original = File.ReadAllText(csprojPath);
            var originalVersion = GetVersion(original);

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
            process.WaitForExit();

            var updated = File.ReadAllText(csprojPath);
            var updatedVersion = GetVersion(updated);

            // Assert
            Assert.NotEqual(originalVersion, updatedVersion);
        }

        private string GetVersion(string csprojContent)
        {
            var start = csprojContent.IndexOf("<Version>") + "<Version>".Length;
            var end = csprojContent.IndexOf("</Version>");
            return csprojContent.Substring(start, end - start).Trim();
        }
    }
}
