// Copyright (c) Chris Pulman. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Spectre.Console.Rx.Json.Syntax;

internal abstract class JsonSyntaxVisitor<T>
{
    public abstract void VisitObject(JsonObject syntax, T context);

    public abstract void VisitArray(JsonArray syntax, T context);

    public abstract void VisitMember(JsonMember syntax, T context);

    public abstract void VisitNumber(JsonNumber syntax, T context);

    public abstract void VisitString(JsonString syntax, T context);

    public abstract void VisitBoolean(JsonBoolean syntax, T context);

    public abstract void VisitNull(JsonNull syntax, T context);
}
