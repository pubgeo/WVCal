// Copyright 2017 The Johns Hopkins University Applied Physics Laboratory.
// Licensed under the MIT License. See LICENSE.txt in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WVCal {
	//utilities for parsing command-line arguments
	public static class CommandUtilities {
		//convert input args[] to arguments (e.g. -input) and zero or more corresponding parameters (e.g. 100)
		// arguments start with "-" and are not a number, all others are parameters
		public static CommandArgField[] parseArgs(string[] args) {
			List<CommandArgField> argsList = new List<CommandArgField>();
			for (int i = 0; i < args.Length; i++) {
				if (args[i][0] == '-' && IsNumber(args[i]) == false) {//if is argument
					//create storage for argument and its parameters
					CommandArgField arg = new CommandArgField();
					arg.argname = args[i];
					List<string> parametersList = new List<string>();

					//read parameters until a new argument is found
					while (!(i + 1 >= args.Length || (args[i + 1][0] == '-' && IsNumber(args[i + 1]) == false))) {
						parametersList.Add(args[i + 1]);
						i++;
					}
					arg.parameters = parametersList.ToArray();
					argsList.Add(arg);
				}
			}

			return argsList.ToArray();
		}


		public static bool IsNumber(String s) {
			for (int i = 0; i < s.Length; i++) {
				if (Char.IsDigit(s, i) == false && s[i] != '-' && s[i] != '.') return false;
			}

			return true;
		}
	}

	public class CommandArgField {
		public string argname;//e.g. -input
		public string[] parameters;//e.g. 100
	}
}
