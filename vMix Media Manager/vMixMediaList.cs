using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace vMix_Media_Manager
{
    class vMixMediaList
    {
        BindingList<vMixInput> inputs = new BindingList<vMixInput>();
        XmlDocument vmixXml;

        string vMixXmlPath;
        
        public BindingList<vMixInput> Inputs
        {
            get
            {
                return inputs;
            }
        }

        public void Open(string path)
        {
            readXml(path);
            vMixXmlPath = path;
        }

        public void Save()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(vMixXmlPath);
            saveFileDialog1.Title = "Save preset Files";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "vmix";
            saveFileDialog1.Filter = "vMix preset files (*.vmix)|*.vmix|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    MessageBox.Show(saveFileDialog1.FileName);
                    vmixXml.Save(saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void readXml(string path)
        {
            vmixXml = new XmlDocument();
            vmixXml.Load(path);

            XmlNodeList nodes = vmixXml.GetElementsByTagName("Input");

            foreach (XmlElement input in nodes)
            {

                if(input.HasAttribute("Type"))
                {
                    int type = int.Parse(input.Attributes.GetNamedItem("Type").InnerText);

                    switch (type)
                    {
                        case 0:
                            addInput(input.Attributes.GetNamedItem("OriginalTitle").InnerText, input.InnerText, input, vMixInputType.Video);
                            break;
                        case 1:
                            addInput(input.Attributes.GetNamedItem("OriginalTitle").InnerText, input.InnerText, input, vMixInputType.Video);
                            break;
                        case 13:
                            addInput(input.Attributes.GetNamedItem("OriginalTitle").InnerText, input.InnerText, input, vMixInputType.Video);
                            break;
                        case 14:
                            addList(input.Attributes.GetNamedItem("OriginalTitle").InnerText, input.Attributes.GetNamedItem("Videos").InnerText, input);
                            break;
                        case 9000:
                            addInput(input.Attributes.GetNamedItem("OriginalTitle").InnerText, input.InnerText, input, vMixInputType.Video); // Added for gtzip files
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void addList(string Title, string videos, XmlElement input)
        {
            string[] videoList = videos.Split('\n');

            foreach (var video in videoList)
            {
                string[] videoParams = video.Split('|');
                addInput(Title,videoParams[0], input, vMixInputType.List);
            }
        }

        private void addInput(string Title, string path, XmlElement input, vMixInputType type)
        {
            vMixInput vmixInput = new vMixInput();
            vmixInput.InputType = type;
            vmixInput.Name = Title;
            vmixInput.Path = path;
            vmixInput.Online = File.Exists(path);
            vmixInput.XmlElement = input;
            inputs.Add(vmixInput);
        }

    }
}
