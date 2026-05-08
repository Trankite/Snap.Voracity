using StarRailDamage.Source.Model.Metadata.Character.Attribute;
using StarRailDamage.Source.Model.Metadata.Character.Damage;
using StarRailDamage.Source.Model.Metadata.Character.Element;
using StarRailDamage.Source.UI.Model.Page.Combat;
using StarRailDamage.Source.UI.Xaml.Control.Panel;
using System.Windows;

namespace StarRailDamage.Source.UI.Xaml.Page
{
    public sealed partial class MockBattlePage : ScopedPage
    {
        public MockBattlePage()
        {
            InitializeComponent();
        }

        public CombatPageModel Model
        {
            get => (CombatPageModel)GetValue(ModelProperty);
            set => SetValue(ModelProperty, value);
        }

        public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof(Model), typeof(CombatPageModel), typeof(MockBattlePage), new PropertyMetadata(new CombatPageModel()));

        public CharacterElement CharacterElement
        {
            get => (CharacterElement)GetValue(CharacterElementProperty);
            set => SetValue(CharacterElementProperty, value);
        }

        public static readonly DependencyProperty CharacterElementProperty = DependencyProperty.Register(nameof(CharacterElement), typeof(CharacterElement), typeof(MockBattlePage), new PropertyMetadata(default(CharacterElement), CharacterElementChangedCallback));

        private static void CharacterElementChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MockBattlePage MockBattlePage)
            {
                MockBattlePage.CharacterElementModel = (MockBattlePage.Model.CharacterElement = MockBattlePage.CharacterElement).GetModel();
            }
        }

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

        public CharacterElementModel CharacterElementModel
        {
            get => (CharacterElementModel)GetValue(CharacterElementModelProperty);
            set => SetValue(CharacterElementModelProperty, value);
        }

        public static readonly DependencyProperty CharacterElementModelProperty = DependencyProperty.Register(nameof(CharacterElementModel), typeof(CharacterElementModel), typeof(MockBattlePage), new PropertyMetadata(default(CharacterElement).GetModel()));

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