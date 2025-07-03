using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.Rendering;
using ZXing;
using System.Drawing;
using Microsoft.Win32;
using System.Windows;
using System.Drawing.Imaging;
using ZXing.QrCode;

namespace QRToolbox.Application.Models {
    internal class QRWithLogoGenerate {
        
        public static ImageSource BitmapToImageSource(Bitmap bitmap) {

            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        } 
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
               
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                Bitmap bitmap = new Bitmap(memoryStream);

                return bitmap;
            }
        }
        public static Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            try { 
            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new Bitmap(outStream);
            }
            return bitmap;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            
        }
        public static BinaryBitmap ConvertBitmapToBinaryBitmap(Bitmap bitmap)
        {
            LuminanceSource source = new BitmapLuminanceSource(bitmap);
            Binarizer binarizer = new HybridBinarizer(source);
            return new BinaryBitmap(binarizer);
        }
        public static Bitmap ResizeLogoForQr(Bitmap qrCodeBitmap, Bitmap logoImage) {
            double naturalLogoWidth = logoImage.Width;
            double naturalLogoHeight = logoImage.Height;
            double koef = 0;
            int logoWidth = 0;
            int logoHeight = 0;

            if (naturalLogoHeight > qrCodeBitmap.Height || naturalLogoWidth > qrCodeBitmap.Width) {
                if (naturalLogoHeight > naturalLogoWidth) {
                    koef = Math.Ceiling(naturalLogoHeight / qrCodeBitmap.Height);
                } else {
                    koef = Math.Ceiling(naturalLogoWidth / qrCodeBitmap.Width);
                    logoWidth = (int)(naturalLogoWidth / koef);
                    logoHeight = (int)(naturalLogoHeight / koef);
                }
            } else {
                if (naturalLogoHeight > naturalLogoWidth) {
                    koef = Math.Round(qrCodeBitmap.Height / naturalLogoHeight);
                } else {
                    koef = Math.Round(qrCodeBitmap.Width / naturalLogoWidth);
                    logoWidth = (int)(naturalLogoWidth * koef);
                    logoHeight = (int)(naturalLogoHeight * koef);
                }
            }

            Bitmap logo = new Bitmap(logoImage, logoWidth / 2, logoHeight / 2);
            return logo;
        }
        public static Bitmap CreateQRCode(string TextBox) {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            EncodingOptions encodingOptions = new EncodingOptions() { Width = 150, Height = 150, Margin = 0, PureBarcode = false };
            encodingOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
            barcodeWriter.Renderer = new BitmapRenderer();
            barcodeWriter.Options = encodingOptions;
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            Bitmap QrCodeBitmap = barcodeWriter.Write(TextBox);
            return QrCodeBitmap;
        }
        public static Bitmap ChangeColor(Bitmap bitmap, System.Drawing.Color newColor) {
            for (int i = 0; i < bitmap.Width; i++) {
                for (int j = 0; j < bitmap.Height; j++) {
                    if (bitmap.GetPixel(i, j) == System.Drawing.Color.FromArgb(255, 0, 0, 0)) {
                        bitmap.SetPixel(i, j, newColor);
                    }
                }
            }

            return bitmap;
        }
        public static Bitmap OpenImage()
        {
            Bitmap newLogo = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP; *.JPG; *.GIF; *.PNG)|*.BMP; *.JPG; *.GIF; *.PNG";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    newLogo = new Bitmap(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading the image: " + ex.Message);
                }
            }

            return newLogo;
        }
        public static void SaveImage(BitmapSource image)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPEG Image|*.jpg";
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(fileStream);
                }
            }
        }
        public static string ScanQrCode(BitmapSource QrCodeImage) {   
            QRCodeReader reader = new QRCodeReader();
            using (var bitmap = QRWithLogoGenerate.BitmapFromSource(QrCodeImage)) {
                var binaryBitmap = QRWithLogoGenerate.ConvertBitmapToBinaryBitmap(bitmap);
                var result = reader.decode(binaryBitmap);
                if (result != null) {
                    return result.Text;
                } else {
                    MessageBox.Show("Не найдено QR-кода.");
                    return null;
                }
            }
        }
        public static string SaveBitmapImageToTempFile(BitmapImage bitmapImage)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".png");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(memoryStream);
                File.WriteAllBytes(tempPath, memoryStream.ToArray());
            }

            return tempPath;
        }
        public static Image ConnectQrWithLogo(Bitmap qrCode, Image logo) {
            Graphics g = Graphics.FromImage(qrCode);
            g.DrawImage(logo, new System.Drawing.Point((qrCode.Width - logo.Width) / 2, (qrCode.Height - logo.Height) / 2));
            return qrCode; 
        }
        public static BitmapImage ImageToBitmapImage(Image Image) {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                Image.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
        public static Image AddWhiteBackground(Bitmap logo)
        {
            Bitmap newImage = new Bitmap(logo.Width, logo.Height);
            using (Graphics g = Graphics.FromImage(newImage))
            {
                g.FillRectangle(System.Drawing.Brushes.White, 0, 0, newImage.Width, newImage.Height);
                g.DrawImage(logo, 0, 0, logo.Width, logo.Height);
            }
            return newImage;
        }
    }
}
