using Common.Source.Model.StarRail.Character.Element;
using StarRail.Source.Model.Character.Attribute;
using StarRail.Source.Model.Character.Damage;
using System.Windows;

namespace StarRail.Source.UI.Page
{
    public sealed partial class MockBattlePage : UIDesign.Source.UI.Panel.Page
    {
        public MockBattlePage()
        {
            InitializeComponent();
            Model = new MockBattlePageModel();
        }

        public MockBattlePageModel Model
        {
            get => (MockBattlePageModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(MockBattlePageModel), typeof(MockBattlePage));

        public CharacterElement CharacterElement
        {
            get => (CharacterElement)GetValue(CharacterElementProperty);
            set => SetValue(CharacterElementProperty, value);
        }

        public static readonly DependencyProperty CharacterElementProperty = DependencyProperty.Register(nameof(CharacterElement), typeof(CharacterElement), typeof(MockBattlePage));

        public CharacterAttributeModel CharacterAttributeModel
        {
            get => (CharacterAttributeModel)GetValue(CharacterAttributeModelProperty);
            set => SetValue(CharacterAttributeModelProperty, value);
        }

        public static readonly DependencyProperty CharacterAttributeModelProperty = DependencyProperty.Register(nameof(CharacterAttributeModel), typeof(CharacterAttributeModel), typeof(MockBattlePage), new PropertyMetadata(default(CharacterAttributeModel), CharacterAttributeModelChangedCallback));

        private static void CharacterAttributeModelChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MockBattlePage MockBattlePage)
            {
                MockBattlePage.Model.CharacterAttributeModel = MockBattlePage.CharacterAttributeModel;
            }
        }

        public CharacterDamageModel CharacterDamageModel
        {
            get => (CharacterDamageModel)GetValue(CharacterDamageModelProperty);
            set => SetValue(CharacterDamageModelProperty, value);
        }

        public static readonly DependencyProperty CharacterDamageModelProperty = DependencyProperty.Register(nameof(CharacterDamageModel), typeof(CharacterDamageModel), typeof(MockBattlePage));

        public CharacterDamageModel ComparedDamageModel
        {
            get => (CharacterDamageModel)GetValue(ComparedDamageModelProperty);
            set => SetValue(ComparedDamageModelProperty, value);
        }

        public static readonly DependencyProperty ComparedDamageModelProperty = DependencyProperty.Register(nameof(ComparedDamageModel), typeof(CharacterDamageModel), typeof(MockBattlePage));
    }
}