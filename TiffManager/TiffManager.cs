using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffManager
{
    public class TiffManager
    {
        public static bool Split(string filepath,string destinationfolder)
        {
            try
            {
                if (!System.IO.File.Exists(filepath))
                {
                    throw new FileNotFoundException("Input file not found");
                }

                int pages;

                System.Drawing.Image image = System.Drawing.Image.FromFile(filepath);

                string filename = System.IO.Path.GetFileNameWithoutExtension(filepath);
                string filext = System.IO.Path.GetExtension(filepath);


                pages = image.GetFrameCount(System.Drawing.Imaging.FrameDimension.Page);

                for (int index = 0; index < pages; index++)
                {
                    image.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Page, index);
                    image.Save(string.Format("{0}//{1}_{2}{3}", destinationfolder, filename, (index + 1).ToString().PadLeft(5,'0'), filext));
                }

                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public static bool Merge(string outputFilename, string sourceFiles, char delemiter=',')
        {
            System.Drawing.Imaging.ImageCodecInfo codec = null;
            string[] sourceFilesArray = sourceFiles.Trim().TrimEnd(delemiter).Split(delemiter);

            foreach (System.Drawing.Imaging.ImageCodecInfo cCodec in System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders())
            {
                if (cCodec.CodecName == "Built-in TIFF Codec")
                    codec = cCodec;
            }

            try
            {

                System.Drawing.Imaging.EncoderParameters imagePararms = new System.Drawing.Imaging.EncoderParameters(1);
                imagePararms.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)System.Drawing.Imaging.EncoderValue.MultiFrame);

                if (sourceFilesArray.Length == 1)
                {
                    if(!System.IO.File.Exists((string)sourceFilesArray[0]))
                    {
                        throw new FileNotFoundException(sourceFilesArray[0] + " not found");
                    }
                    System.IO.File.Copy((string)sourceFilesArray[0], outputFilename, true);

                }
                else if (sourceFilesArray.Length >= 1)
                {
                    if (!System.IO.File.Exists((string)sourceFilesArray[0]))
                    {
                        throw new FileNotFoundException(sourceFilesArray[0] + " not found");
                    }

                    System.Drawing.Image DestinationImage = (System.Drawing.Image)(new System.Drawing.Bitmap((string)sourceFilesArray[0]));

                    DestinationImage.Save(outputFilename, codec, imagePararms);

                    imagePararms.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)System.Drawing.Imaging.EncoderValue.FrameDimensionPage);


                    for (int i = 1; i < sourceFilesArray.Length; i++)
                    {
                        if (!System.IO.File.Exists((string)sourceFilesArray[i]))
                        {
                            throw new FileNotFoundException(sourceFilesArray[i] + " not found");
                        }

                        System.Drawing.Image img = (System.Drawing.Image)(new System.Drawing.Bitmap((string)sourceFilesArray[i]));

                        DestinationImage.SaveAdd(img, imagePararms);
                        img.Dispose();
                    }

                    imagePararms.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)System.Drawing.Imaging.EncoderValue.Flush);
                    DestinationImage.SaveAdd(imagePararms);
                    imagePararms.Dispose();
                    DestinationImage.Dispose();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
