using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Plugin.Media;

namespace DemoCamera
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Android.Widget.Button btnCapturarFoto;
        Android.Widget.ImageView imgPhoto;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            imgPhoto = FindViewById<Android.Widget.ImageView>(Resource.Id.imgPhoto);
            btnCapturarFoto = FindViewById<Android.Widget.Button>(Resource.Id.btnCapturarFoto);
            btnCapturarFoto.Click += BtnCapturarFoto_Click;

        }

        async void capturarFoto()
        {
            // Se inicializa el plugin para tomar fotos
            await CrossMedia.Current.Initialize();

            // Se toma la foto por medio del plugin y de forma asíncrona
            var file = await CrossMedia.Current.TakePhotoAsync(
                new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    CompressionQuality = 50,
                    Name = string.Format("Imagen {0}.jpg", System.Guid.NewGuid()),
                    Directory = "DemoCamera",
                    SaveToAlbum = true
                });

            if (file == null)
                return;

            // se leen todos los bytes del archivo de la foto que se acaba de tomar.
            byte[] photoBytesArray = System.IO.File.ReadAllBytes(file.Path);
            // Se genera el bitmap a partir de los bytes de la foto
            Android.Graphics.Bitmap bitmap = 
                Android.Graphics.BitmapFactory.DecodeByteArray(photoBytesArray,0,photoBytesArray.Length);
            this.imgPhoto.SetImageBitmap(bitmap);

        }

        private void BtnCapturarFoto_Click(object sender, System.EventArgs e)
        {
            capturarFoto();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}