SVN Revision Labeller is a plugin for [CruiseControl.NET](http://ccnet.thoughtworks.com) (CC.NET) that allows you to generate version labels for your builds, based upon the revision number of your [Subversion](http://subversion.tigris.org) working copy. This can be customised with a prefix and/or major/minor version numbers.

## Important Update! ##
I have essentially stopped development work on SVN Revision Labeller; I don't have as much time as I used to, and CC.NET's AssemblyVersionLabeller now does enough for me to use it in place of my own code.

However, many thanks to [Tess Ashpool](https://github.com/neutmute), who has stepped into the breech, and forked into Github. This has the added benefit of allowing the community to work more actively on the code, without me being a roadblock on updates. For the future, please use https://github.com/neutmute/SvnRevisionLabeller.

## Requirements ##

### Mandatory ###
  * CC.NET 1.5 - the plugin has been compiled against 1.5 RTM, and due to breaking changes in CC.NET, will not run on versions 1.4.3 and prior. If you cannot upgrade your version of CruiseControl.NET, please use the older v1.0.3 release;
  * .NET Framework 2.0 - actually a requirement for CC.NET, but it doesn't hurt to state the obvious;
  * Subversion client - hopefully you'll have this installed anyway as part of your CC.NET configuration
### Optional ###
  * Visual Studio 2008 - for building from source, debugging or adding new features;
  * Visual Studio 2003 - if you're feeling brave, and want to recompile for .NET 1.1 and versions of CC.NET prior to 1.3.

## Installation ##

The precompiled build provides a release build only. If you want to run a debug build, you will need to grab a copy of the source and build it yourself. Otherwise, just drop the file `ccnet.SvnRevisionLabeller.plugin.dll` into the CC.NET `server` folder. The CC.NET configuration will need to be updated, and the server optionally restarted (depending on your configuration).

## Configuration ##

Below is a sample configuration for svnRevisionLabeller, showing the mandatory fields:

```
<labeller type="svnRevisionLabeller">
	<major>7</major>
	<minor>11</minor>
	<url>svn://localhost/repository/trunk</url>
</labeller>
```

The following sample configuration shows the complete set of fields:

```
<labeller type="svnRevisionLabeller">
	<major>8</major>
	<minor>2</minor>
	<build>0</build>
	<pattern>Prerelease {major}.{minor}.{build}.{revision}</pattern>
	<incrementOnFailure>false</incrementOnFailure>
	<resetBuildAfterVersionChange>false</resetBuildAfterVersionChange>
	<url>https://localhost/repository/branches/dev-project</url>
	<executable>C:\Svn\Bin\svn.exe</executable>
	<username>ccnetuser</username>
	<password>ccnetpassword</password>
	<startDate>25/10/2010</startDate>
</labeller>
```

## Usage ##

When CruiseControl.NET begins a project build, it generates a label for the build and stores it in the property `CCNetLabel` - this property can then be used by NAnt or MSBuild to generate the `AssemblyInfo.cs` for your assemblies, so that CC.NET displays as its label the same version that the assemblies are built with. So, if the configuration for the labeller is set as:

```
<labeller type="svnRevisionLabeller">
	<major>7</major>
	<minor>11</minor>
	<url>svn://localhost/repository/trunk</url>
</labeller>
```

and the latest Subversion revision number is 920, the CCNetLabel will be set to 7.11.0.920. Forcing a build without any changes to the repository will not make any changes to the label. A subsequent commit to the repository would then set the label to 7.11.0.921, and so on.

If you want to generate a more complex label, you use the Pattern field. This contains a number of tokens for the Major, Minor, Build, Revision and Rebuilt numbers,and you can effectively create any label you want. For instance:

```
<labeller type="svnRevisionLabeller">
	<major>1</major>
	<minor>2</minor>
	<pattern>Labelling is as easy as {major} - {minor} - 3 - {revision}. See?</pattern>
	<url>svn://localhost/repository/trunk</url>
</labeller>
```

and the current revision is 4, then the generated build label be "Labelling is as easy as 1 - 2 - 3 - 4. See?"

The available tokens are:

  * {major} - the major build number
  * {minor} - the minor build number
  * {build} - the build number
  * {revision} - the revision number
  * {rebuild} - the number of times the build has been rebuilt (i.e. a forced build)
  * {date} - the number of days elapsed since the date specified in the startDate field
  * {msrevision} - the revision number that Microsoft calculates - the number of seconds since midnight, divided by two

## History ##

  * 3.1.0.32163
    * NEW - added new {date} token to allow build numbers to be based on days elapsed since a given date;
    * NEW - add new {msrevision} token to allow revision numbers to be based on the Microsoft calcuation

  * 3.0.0.24490
    * FIX - now runs against CC.NET v1.5

  * 2.0.0.20990
    * FIX - now runs against CC.NET v1.4.4 RC2;
    * NEW - greater control over the formatting of the build label (patch provided by fezguy); the prefix and postfix fields have been removed from configuration, since they are now replaced by the Pattern field, and by default, rebuilds are not counted. To reproduce the original behaviour of the plugin, you would want a Pattern similar to "{major}.{minor}.{revision}.{rebuilt}", so that successive forced builds without a new Subversion commit increments the version number by 1;
  * 1.0.3.25899
    * FIX - the username and password attributes are swapped around (fix provided by Tony Mitchell);
  * 1.0.2.16573
    * Now built against CC.NET v1.3, and tested against CC.NET v1.3
    * FIX - the revision number does not increase on successive builds if a prefix is specified (fix provided by Mike Usner);
    * FIX - if no working copy exists, then the labeller will not be able to work out what the current revision is, and throw an exception (fix provided by Matteo Tontini);
  * 1.0.1.21635
    * Built against CC.NET v1.1, and tested against CC.NET v1.1 and v2.0;
    * initial public release;