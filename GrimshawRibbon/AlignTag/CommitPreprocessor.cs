using Autodesk.Revit.DB;
using System.Collections;
using System.Collections.Generic;

namespace AlignTag
{
  public class CommitPreprocessor : IFailuresPreprocessor
  {
    public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
    {
      IList<FailureMessageAccessor> failureMessageAccessorList = (IList<FailureMessageAccessor>) new List<FailureMessageAccessor>();
      using (IEnumerator<FailureMessageAccessor> enumerator = ((IEnumerable<FailureMessageAccessor>) failuresAccessor.GetFailureMessages()).GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          FailureMessageAccessor current = enumerator.Current;
          FailureDefinitionId failureDefinitionId = current.GetFailureDefinitionId();
          if (FailureDefinitionId.op_Equality(failureDefinitionId, BuiltInFailures.RoomFailures.get_RoomTagNotInRoom()) || FailureDefinitionId.op_Equality(failureDefinitionId, BuiltInFailures.RoomFailures.get_RoomTagNotInRoomToArea()) || FailureDefinitionId.op_Equality(failureDefinitionId, BuiltInFailures.RoomFailures.get_RoomTagNotInRoomToRoom()) || FailureDefinitionId.op_Equality(failureDefinitionId, BuiltInFailures.RoomFailures.get_RoomTagNotInRoomToSpace()))
            failuresAccessor.DeleteWarning(current);
        }
      }
      return (FailureProcessingResult) 0;
    }
  }
}
