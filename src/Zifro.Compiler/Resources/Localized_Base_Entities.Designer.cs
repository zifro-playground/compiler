﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zifro.Compiler.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Localized_Base_Entities {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Localized_Base_Entities() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Zifro.Compiler.Resources.Localized_Base_Entities", typeof(Localized_Base_Entities).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ett heltal kan inte indexeras..
        /// </summary>
        internal static string Ex_Int_IndexGet {
            get {
                return ResourceManager.GetString("Ex_Int_IndexGet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Det går inte att sätta inre värde på ett heltal genom indexering..
        /// </summary>
        internal static string Ex_Int_IndexSet {
            get {
                return ResourceManager.GetString("Ex_Int_IndexSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ett heltalsvärde kan inte köras som en funktion..
        /// </summary>
        internal static string Ex_Int_Invoke {
            get {
                return ResourceManager.GetString("Ex_Int_Invoke", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gick inte att läsa egenskapen &apos;{1}&apos; från heltalet. Heltal har inga egenskaper..
        /// </summary>
        internal static string Ex_Int_PropertyGet {
            get {
                return ResourceManager.GetString("Ex_Int_PropertyGet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Det går inte att ändra egenskapen &apos;{1}&apos; på ett heltal..
        /// </summary>
        internal static string Ex_Int_PropertySet {
            get {
                return ResourceManager.GetString("Ex_Int_PropertySet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to En textsträng kan endast indexeras av heltal, inte av typen &apos;{2}&apos;..
        /// </summary>
        internal static string Ex_String_IndexGet_InvalidType {
            get {
                return ResourceManager.GetString("Ex_String_IndexGet_InvalidType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gick inte att läsa bokstaven på plats &apos;{2}&apos; då den är utanför textsträngens längd ({1})..
        /// </summary>
        internal static string Ex_String_IndexGet_OutOfRange {
            get {
                return ResourceManager.GetString("Ex_String_IndexGet_OutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Det går inte att ändra enstaka bokstäver i en textsträng genom indexering..
        /// </summary>
        internal static string Ex_String_IndexSet {
            get {
                return ResourceManager.GetString("Ex_String_IndexSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to En textsträng kan inte köras som en funktion..
        /// </summary>
        internal static string Ex_String_Invoke {
            get {
                return ResourceManager.GetString("Ex_String_Invoke", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gick inte att läsa egenskapen &apos;{2}&apos; från textsträngen..
        /// </summary>
        internal static string Ex_String_PropertyGet {
            get {
                return ResourceManager.GetString("Ex_String_PropertyGet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Det går inte att ändra egenskapen &apos;{2}&apos; på en textsträng..
        /// </summary>
        internal static string Ex_String_PropertySet {
            get {
                return ResourceManager.GetString("Ex_String_PropertySet", resourceCulture);
            }
        }
    }
}
