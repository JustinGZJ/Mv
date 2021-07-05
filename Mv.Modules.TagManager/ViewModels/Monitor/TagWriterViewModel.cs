using DataService;

namespace Mv.Modules.TagManager.ViewModels.Monitor
{
    public class TagWriterViewModel
    {

        public TagWriterViewModel(ITag tag)
        {
            Title = tag.GetTagName();
            Value = tag.ToString();
        }
        public string Title { get; set; }

        public string Value { get; set; }

    }
}
