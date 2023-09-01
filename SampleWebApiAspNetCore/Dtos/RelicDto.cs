namespace SampleWebApiAspNetCore.Dtos
{
    public class RelicDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int Attack { get; set; }
        public DateTime Created { get; set; }
    }
}
