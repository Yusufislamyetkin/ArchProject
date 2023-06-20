using Arch.EntityLayer.Entities;

namespace Arch.UI.Models
{
    public class DesignerFiles
    {
        public string DesignerId { get; set; }
        public int SelectedOption { get; set; }
        public string Name { get; set; }
        public List<ProjectFilePath> Files { get; set; }
    }
}
