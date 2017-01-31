﻿using Android.App;
using Android.Widget;
using Android.OS;
using SQLite;

namespace MyClassRoom
{
    [Activity(Label = "MyClassRoom", MainLauncher = true, Icon = "@drawable/icon"),]
    public class MainActivity : Activity
    {
        int count = 1;
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button button = FindViewById<Button>(Resource.Id.btnCameralogin);
                _imageView = FindViewById<ImageView>(Resource.Id.imageViewAuth);
                button.Click += TakeAPicture;
            }
        }

        private void CreateDirectoryForPictures()
        {
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "CameraUserAuth");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities =
                PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Make it available in the gallery

            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            Uri contentUri = Uri.FromFile(App._file);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            // Display in ImageView. We will resize the bitmap to fit the display.
            // Loading the full sized image will consume to much memory
            // and cause the application to crash.

            int height = Resources.DisplayMetrics.HeightPixels;
            int width = _imageView.Height;
            App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
            if (App.bitmap != null)
            {
                _imageView.SetImageBitmap(App.bitmap);
                App.bitmap = null;
            }

            // Dispose of the Java side bitmap.
            GC.Collect();
        }

        public static class BitmapHelpers
        {
            public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
            {
                // First we get the the dimensions of the file on disk
                BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
                BitmapFactory.DecodeFile(fileName, options);

                // Next we calculate the ratio that we need to resize the image by
                // in order to fit the requested dimensions.
                int outHeight = options.OutHeight;
                int outWidth = options.OutWidth;
                int inSampleSize = 1;

                if (outHeight > height || outWidth > width)
                {
                    inSampleSize = outWidth > outHeight
                                       ? outHeight / height
                                       : outWidth / width;
                }

                // Now we will load the image and have BitmapFactory resize it for us.
                options.InSampleSize = inSampleSize;
                options.InJustDecodeBounds = false;
                Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

                return resizedBitmap;
            }
            //15/07/2016 - Madoda
            //taken a picture and saved it on device
            //need to code face recognition on the newly taken picture against the one stored in the server and DB 
            //then store the latest picture and remove it from the device and delet the old picture from the server 
        }
    }

}

        //Database connectivity
//        public interface ISQLite
//        {
//            SQLiteConnection GetConnection();
//        }

//        public TodoItemDatabase()
//        {
//            database = DependencyService.Get<ISQLite>().GetConnection();
//            database.CreateTable<TodoItem>();
//        }

//        public IEnumerable<TodoItem> GetItems()
//        {
//            return (from i in database.Table<TodoItem>() select i).ToList();
//        }
//        public IEnumerable<TodoItem> GetItemsNotDone()
//        {
//            return database.Query<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
//        }
//        public TodoItem GetItem(int id)
//        {
//            return database.Table<TodoItem>().FirstOrDefault(x => x.ID == id);
//        }
//        public int DeleteItem(int id)
//        {
//            return database.Delete<TodoItem>(id);
//        }
//    }
//}

