Prometheus
==========

[Prometheus](https://github.com/liammclennan/Prometheus) is a pre-alpha project (most of mine stay that way) that converts an F# assembly to a TypeScript file containing class equivalents of all the records in the F# assembly.

Usage
-----

	Prometheus path\to\my\assembly.dll

Example 
-------

The following F# module

	namespace MyModels

	    type Address = { number: int; street: string }
	    type Person = { name: string; age: int; address: Address }

will be converted to

	module MyModels { 
	    export class Address {
			constructor(public number: number, public street: string) {}
		}
	}

	module MyModels { 
		export class Person {
			constructor(public name: string, public age: number, public address: Address) {}
		}
	}

