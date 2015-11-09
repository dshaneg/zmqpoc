# ZeroMQ POC
## Actually, just working through [The Guide](http://zguide.zeromq.org/page:all) 

Working through the sample code from the guide in C#, making mostly very small tweaks here and there.
The solution is broken down by chapter and then exercise. Most of the code came from the [Metadings](https://github.com/metadings/zguide/tree/master/examples/C%23) examples, but I've separated each exercise
into its own console app or set of console apps instead of all being run under a single controlling app.

I'm using the "[ZeroMQ](https://github.com/zeromq/clrzmq4/)" nuget package for my projects,
even though it's a tad dirty in that it adds the c libraries to your project as content and you need
to set the dlls' "Copy to Output Directory" property to "Copy if Newer" manually to make it work.

The solution contains batch files for launching multiple apps for exercises where this is warranted.

I'm still working through the guide so this is a work in progress.
