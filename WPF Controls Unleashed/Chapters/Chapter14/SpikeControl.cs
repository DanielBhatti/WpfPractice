using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chapters.Chapter14
{
	public class SpikeControl : ContentControl, ICommandSource
	{
		public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
			"Command", typeof(ICommand), typeof(SpikeControl), new PropertyMetadata(null, OnCommandChanged));

		public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
			"CommandParameter", typeof(object), typeof(SpikeControl));

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public object CommandParameter
		{
			get { return (object)GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public IInputElement CommandTarget
		{
			get;
			set;
		}

		private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			SpikeControl control = d as SpikeControl;
			control.HookUpCommand(e.OldValue as ICommand, e.NewValue as ICommand);
		}

		public void DoSomethingToInvokeCommand()
		{
			// Some action that results in a Command being fired

			InvokeCommand();
		}

		private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
		{
			if (oldCommand != null)
			{
				oldCommand.CanExecuteChanged -= Command_CanExecuteChanged;
			}
			if (newCommand != null)
			{
				newCommand.CanExecuteChanged += Command_CanExecuteChanged;
			}
		}

		private void Command_CanExecuteChanged(object sender, EventArgs e)
		{
			if (Command != null)
			{
				RoutedCommand command = Command as RoutedCommand;

				// RoutedCommand.
				if (command != null)
				{
					IInputElement inputElt = CommandTarget;
					if (inputElt == null) inputElt = this;

					if (command.CanExecute(CommandParameter, inputElt))
					{
						IsEnabled = true;
					}
					else
					{
						IsEnabled = false;
					}
				}
				// Not a RoutedCommand.
				else
				{
					if (Command.CanExecute(CommandParameter))
					{
						IsEnabled = true;
					}
					else
					{
						IsEnabled = false;
					}
				}
			}
		}

		private void InvokeCommand()
		{
			if (Command != null)
			{
				RoutedCommand command = Command as RoutedCommand;
				if (command != null)
				{
					command.Execute(CommandParameter, CommandTarget);
				}
				else
				{
					Command.Execute(CommandParameter);
				}
			}
		}
	}
}