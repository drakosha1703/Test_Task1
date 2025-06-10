using System.ComponentModel.DataAnnotations.Schema;

namespace test_task.models
{
    public class Account
        
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int FounderId { get; set; }
        [ForeignKey("FounderId")]
        public Founders Founders { get; set; }
    }
}
