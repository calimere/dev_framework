using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dev_framework.Components.Model.Card
{
    public class CardViewModel : ViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public CardHeaderModel Header { get; set; }
        public CardBodyModel Body { get; set; }

        public CardViewModel()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Title = Id;
            Header = new CardHeaderModel() { Id = Id, Title = Title };
            Body = new CardBodyModel() { Id = Id, Title = Title };
        }
        public CardViewModel(string id, string title, bool isCollapsed = false) : base()
        {
            Id = id;
            Title = title;
            Header = new CardHeaderModel() { Id = Id, Title = Title };
            Body = new CardBodyModel() { Id = Id, Title = Title };
        }

        public CardViewModel(string id, string title, string view, object model = null, bool isCollapsed = false) : base()
        {
            Id = id;
            Title = title;
            Header = new CardHeaderModel() { Id = Id, Title = Title, };
            Body = new CardBodyModel() { Id = Id, Title = Title, View = view, Model = model };
        }
    }

    public abstract class CardModel : Model
    {
        public string BodyId { get { return $"{Id}-body"; } }
        public bool? IsCollapsed { get; set; }

        public CardModel()
        {
            IsCollapsed = false;
        }
    }
    public class CardHeaderModel : CardModel
    {
        public string HeaderId { get { return $"{Id}-header"; } }
        public bool IsCollapsable { get; set; }
        public string[] Icons { get; set; }

        public CardHeaderModel() : base()
        {
            IsCollapsable = false;
            Icons = new string[0];
        }
    }
    public class CardBodyModel : CardModel
    {
        public string View { get; set; }
        public object Model { get; set; }
    }
}
