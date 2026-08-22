using UIDesign.Source.Factory.NotifyPropertyChanged;
using UIDesign.Source.UI.Panel;

namespace UIDesign.Source.UI.Control
{
    public class TabItemModel : NotifyPropertyChangedFactory
    {
        private string _Header = string.Empty;

        private Page? _Content;

        public TabItemModel(string header)
        {
            Header = header;
        }

        public TabItemModel(string header, Page? content) : this(header)
        {
            Content = content;
        }

        public string Header
        {
            get => _Header;
            set => SetField(ref _Header, value);
        }

        public Page? Content
        {
            get => _Content;
            set => SetField(ref _Content, value);
        }
    }
}