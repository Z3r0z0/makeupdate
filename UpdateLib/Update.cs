using ArgumentsLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UpdateModelLib;

namespace UpdateLib
{
    public class Update : IDisposable
    {
        public event WriteLine UpdateMessage;

        private UpdateConfig _config;

        private Arguments _arguments;
        private UpdateModel _model;
        private Reflector _reflector;

        public Update(UpdateConfig config, IEnumerable<string> args)
        {
            this._config = config;
            this._arguments = new Arguments(config.MarshalerPath, config.Schema, args);
            this._reflector = new Reflector(config.ModelPath);

        }

        public void ExecuteUpdate()
        {
            UpdateMessage?.Invoke("Trying to load update type!");

            LoadUpdateModel();

            ExecuteUpdateModel();
        }

        private void LoadUpdateModel()
        {
            if (!string.IsNullOrWhiteSpace(this._config.Model))
                _model = this._reflector.GetInstance(this._config.Model);
            else
                _model = this._reflector.GetInstance(this._arguments.GetValue<string>("using"));

            this._model.UpdateMessage += this.UpdateMessage;
            this._model.Arguments = this._arguments;
        }

        private void ExecuteUpdateModel()
        {
            this._model.BeforeUpdate();
            this._model.Update();
            this._model.AfterUpdate();
        }

        public void Dispose()
        {
            this._model.UpdateMessage -= this.UpdateMessage;
        }
    }
}
