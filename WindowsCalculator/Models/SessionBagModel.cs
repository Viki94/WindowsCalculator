/* I found this code from https://www.codeproject.com/articles/191422/accessing-asp-net-session-data-using-dynamics.
 * I decided to use a session here for a few reasons.
 * 1) It allows the model to function as a POCO class. This would let the model work on a web app or as a standalone product and the code could be reused.
 * 2) The overhead in using a database was really large for preserving the state of like 3 numbers and a couple booleans.
 * 3) The session expiration matches what the user might expect for a real calculator: it saves the numbers you were using for a while but if you come back 20 minutes later you don't care if calculations expire.
 */

using System.Dynamic;
using System.Web;

public sealed class SessionBagModel : DynamicObject
{
    private static readonly SessionBagModel sessionBagModel;

    static SessionBagModel()
    {
        sessionBagModel = new SessionBagModel();
    }

    private SessionBagModel()
    {
    }

    private HttpSessionStateBase Session
    {
        get { return new HttpSessionStateWrapper(HttpContext.Current.Session); }
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        result = Session[binder.Name];
        return true;
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        Session[binder.Name] = value;
        return true;
    }

    public override bool TryGetIndex(GetIndexBinder
           binder, object[] indexes, out object result)
    {
        int index = (int)indexes[0];
        result = Session[index];
        return result != null;
    }

    public override bool TrySetIndex(SetIndexBinder binder,
           object[] indexes, object value)
    {
        int index = (int)indexes[0];
        Session[index] = value;
        return true;
    }

    public static dynamic Current
    {
        get { return sessionBagModel; }
    }
}