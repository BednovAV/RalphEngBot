using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    public class TheoryLink
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public int GrammarTestId { get; set; }
        public GrammarTest GrammarTest { get; set; }
    }
}