using System;
using System.Collections;
using System.Collections.Generic;
using ArgumentsLib;

namespace UpdateModelLib
{
    public delegate void WriteLine(object o);

    public abstract class UpdateModel
    {
        public abstract event WriteLine UpdateMessage;

        private string _model;

        public abstract void BeforeUpdate();
        public abstract void Update();
        public abstract void AfterUpdate();

        public Arguments Arguments { get; set; }
        public string Model 
        { 
            get => this._model;
            set
            {
                this._model = value.Trim().ToLower();
            }
        }
    }
}
