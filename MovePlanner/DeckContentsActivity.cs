using Android.Views;
using Android.Widget;
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
        private int cardsLeftInDeck = 13;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.deck_contents_activity);

            SetHandlersForDeleteButtons();
        }


        private void SetHandlersForDeleteButtons()
        {
            const int POSITION_OF_DELETE_BTN = 1;
            LinearLayout outerLinearLayout = FindViewById<LinearLayout>(Resource.Id.outerLinerLayout);

            for (int i = 0; i < outerLinearLayout.ChildCount; i++)
            {
                View child = outerLinearLayout.GetChildAt(i);

                if (child is LinearLayout)
                {
                    LinearLayout inner = (LinearLayout) child;
                    ImageView delete_button = (ImageView) inner.GetChildAt(POSITION_OF_DELETE_BTN);
                    delete_button.Click += (sender, e) => 
                    { 
                        outerLinearLayout.RemoveView(inner);
                        this.cardsLeftInDeck--;
                        TextView cardCountDisplay = FindViewById<TextView>(Resource.Id.card_count_display);
                        string cardOrCards = (this.cardsLeftInDeck == 1 ? "card" : "cards");
                        cardCountDisplay.Text = $"{this.cardsLeftInDeck} {cardOrCards} left in deck";
                    };
                }
            }
        }
    }
}
