using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UpdateModelLib;

namespace UpdateLib
{
    public class Reflector
    {
        private readonly string modelPath;

        private IEnumerable<string> filePath;
        private List<Assembly> assemblies;
        private List<Type> types;

        public Reflector(string modelPath)
        {
            if (!Directory.Exists(modelPath))
                throw new LibraryException(ErrorCode.GLOBAL, $"Model Directory: {modelPath} not found!");

            this.modelPath = modelPath;
            this.assemblies = new List<Assembly>();
            this.types = new List<Type>();

            SetFilePaths();
            LoadAssemblies();
            SetTypes();
        }

        private void SetFilePaths()
        {
            filePath = Directory.GetFiles(this.modelPath, "*UpdateModelLib.dll");

            if (filePath.Count() == 0)
                throw new LibraryException(ErrorCode.GLOBAL, $"Marshaler Directory: {this.modelPath} does not contain *UpdateModelLib.dll files!");
        }

        private void LoadAssemblies()
        {
            foreach (string path in filePath)
            {
                assemblies.Add(Assembly.LoadFrom(path));
            }
        }

        private void SetTypes()
        {
            foreach (Assembly assembly in assemblies)
            {
                types.Add(assembly.GetType(assembly.DefinedTypes.First().FullName));
            }
        }

        public UpdateModel GetInstance(string model)
        {
            if (model == null)
                throw new LibraryException(ErrorCode.INVALID_MODEL, "UpdateModel modlename is NULL");

            Type correctType = null;

            foreach (Type type in types)
            {
                object instance = Activator.CreateInstance(type);
                PropertyInfo instanceInfo = type.GetProperty("Modle");
                string value = instanceInfo.GetValue(instance).ToString();

                if (value == null)
                    throw new LibraryException(ErrorCode.INVALID_MODEL, $"{type.FullName} is not an UpdateModelLib or modle property is NULL");

                if (model.Trim().ToLower() == value)
                {
                    correctType = type;
                    break;
                }
            }

            return (UpdateModel)Activator.CreateInstance(correctType);
        }
    }
}
