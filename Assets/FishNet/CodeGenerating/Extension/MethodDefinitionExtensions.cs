using MonoFN.Cecil;

namespace FishNet.CodeGenerating.Extension
{


    internal static class MethodDefinitionExtensions
    {

        /// <summary>
        /// Returns a method in the next base class.
        /// </summary>
        public static MethodReference GetMethodReference(this MethodDefinition md, CodegenSession session)
        {
            MethodReference methodRef = session.ImportReference(md);

            //Is generic.
            if (md.DeclaringType.HasGenericParameters)
            {
                GenericInstanceType git = methodRef.DeclaringType.MakeGenericInstanceType();
                MethodReference result = new MethodReference(md.Name, md.ReturnType)
                {
                    HasThis = md.HasThis,
                    ExplicitThis = md.ExplicitThis,
                    DeclaringType = git,
                    CallingConvention = md.CallingConvention,
                };

                return result;
            }
            //Not generic.
            else
            {
                return methodRef;
            }
        }

    }


}