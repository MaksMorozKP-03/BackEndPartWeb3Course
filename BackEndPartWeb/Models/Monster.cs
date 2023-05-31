

using System.Text.Json.Serialization;

namespace BackEndPartWeb.Models
{
    public class Monster
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Clasification Clasification { get; set; }
        public string Description { get; set; }
        public Image? Image { get; set; }
        [JsonIgnore]
        public List<User>? Users { get; set; }


    }
}
