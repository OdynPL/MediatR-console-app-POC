namespace PersonManager.DTO
{
    public class PersonResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
