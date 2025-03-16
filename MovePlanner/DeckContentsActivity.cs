using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovePlanner
{
    [Activity(Label = "@string/deck_contents_activity_title", MainLauncher = false)]
    public class DeckContentsActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.deck_contents_activity);
        }
    }
}
