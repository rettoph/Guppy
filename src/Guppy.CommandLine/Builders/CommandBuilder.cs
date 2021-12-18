using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Guppy.CommandLine.Builders
{
    public sealed class CommandBuilder : IFluentPrioritizable<CommandBuilder>
    {
        #region Public Fields
        public readonly String Name;
        #endregion

        #region Public Properties
        public String Description { get; set; }
        public Int32 Priority { get; set; }
        public List<Argument> Arguments { get; set; }
        public List<Option> Options { get; set; }
        public List<CommandBuilder> SubCommands { get; set; }
        public List<String> Aliases { get; set; }
        public ICommandHandler? DefaultHandler { get; set; }
        #endregion

        #region Constructor
        public CommandBuilder(string name)
        {
            this.Name = name;

            this.Arguments = new List<Argument>();
            this.Options = new List<Option>();
            this.SubCommands = new List<CommandBuilder>();
            this.Aliases = new List<String>();
        }
        #endregion

        #region SetDescription Methods
        public CommandBuilder SetDescription(String description)
        {
            this.Description = description;

            return this;
        }
        #endregion

        #region AddArgument Methods
        public CommandBuilder AddArgument(Argument argument)
        {
            this.Arguments.Add(argument);

            return this;
        }
        #endregion

        #region AddOption Methods
        public CommandBuilder AddOption(Option option)
        {
            this.Options.Add(option);

            return this;
        }
        #endregion

        #region AddCommand Methods
        public CommandBuilder AddSubCommand(String name, Action<CommandBuilder> builder)
        {
            CommandBuilder subCommand = new CommandBuilder(name);
            builder(subCommand);
            this.SubCommands.Add(subCommand);

            return this;
        }
        #endregion

        #region AddAlias Methods
        public CommandBuilder AddAlias(String alias)
        {
            this.Aliases.Add(alias);

            return this;
        }
        #endregion

        #region SetHandler Methods
        public CommandBuilder SetDefaultHandler(ICommandHandler handler)
        {
            this.DefaultHandler = handler;

            return this;
        }
        #endregion

        #region Build Methods
        internal Command Build()
        {
            Command command = new Command(this.Name, this.Description);
            command.Handler = this.DefaultHandler;

            foreach (String alias in this.Aliases)
            {
                command.AddAlias(alias);
            }

            foreach (Option option in this.Options)
            {
                command.AddOption(option);
            }

            foreach (Argument argument in this.Arguments)
            {
                command.AddArgument(argument);
            }

            foreach (CommandBuilder subCommand in this.SubCommands)
            {
                command.AddCommand(subCommand.Build());
            }

            return command;
        }
        #endregion
    }
}
