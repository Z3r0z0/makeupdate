using ArgumentsLib;
using System;
using System.Collections.Generic;
using UpdateModelLib;

namespace FileUpdateModelLib
{
    public class FileUpdateModel : UpdateModel
    {
        private const string model = "File";

        public FileUpdateModel()
        {
            base.Model = model;
        }

        public override event WriteLine UpdateMessage;

        public override void BeforeUpdate()
        {
            this.UpdateMessage("Before Update");
        }

        public override void Update()
        {
            this.UpdateMessage(base.Arguments.GetValue<bool>("enabled"));
            this.UpdateMessage("Update");
        }

        public override void AfterUpdate()
        {
            this.UpdateMessage("After Update");
        }
    }
}
