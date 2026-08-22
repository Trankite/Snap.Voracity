using Common.Source.Model.StarRail.Character.Element;
using StarRail.Source.Model.Character.Attribute;
using UIDesign.Source.Factory.NotifyPropertyChanged;

namespace StarRail.Source.UI.Page
{
    public class MockBattlePageModel : NotifyPropertyChangedFactory
    {
        private CharacterElement _CharacterElement;

        private CharacterAttributeModel _CharacterAttributeModel = new();

        public CharacterElement CharacterElement
        {
            get => _CharacterElement;
            set => SetField(ref _CharacterElement, value);
        }

        public CharacterAttributeModel CharacterAttributeModel
        {
            get => _CharacterAttributeModel;
            set => SetField(ref _CharacterAttributeModel, value);
        }
    }
}