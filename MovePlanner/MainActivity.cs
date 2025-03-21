using Android.Content;

namespace MovePlanner
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_main);


            Button getStartedBtn = FindViewById<Button>(Resource.Id.get_started_btn);
            getStartedBtn.Click += (sender, e) =>
            {
                Intent intent = new Intent(this, typeof(DeckContentsActivity));
                StartActivity(intent);
            };
        }
    }
}