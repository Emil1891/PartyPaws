using System;

public class Note
{

    private readonly float time;
    private readonly char buttonName;

    public Note(float time, char buttonName)
    {
        this.time = time;
        this.buttonName = buttonName;
    }

    public float GetTime()
    {
        return time;    
    }

    public char GetButtonName()
    { 
        return buttonName; 
    }

    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }

        if (this == obj)
            return true; 
        
        Note otherNote = (Note) obj;
        return time.Equals(otherNote.time) && buttonName.Equals(otherNote.buttonName); 
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(time, buttonName); 
    }
}
