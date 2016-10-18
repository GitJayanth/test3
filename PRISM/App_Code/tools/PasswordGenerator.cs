using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using HyperComponents.Security.Cryptography;
using System.Text;

public class PasswordGenerator
{
  #region "Private properties"
  private const int DefaultMinimum = 6;
  private const int DefaultMaximum = 10;
  private const int UBoundDigit    = 61;

  private RNGCryptoServiceProvider _Rng;
  private int _MinSize, _MaxSize;
  private bool _HasRepeating, _HasConsecutive, _HasSymbols;
  private string _ExclusionSet;
  private char[] _PwdCharArray = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789`~!@#$%^&*()-_=+[]{}\\|;:'\",<.>/?".ToCharArray();                                        
  #endregion

  #region "Published properties"
  public string Exclusions
  {
    get { return this._ExclusionSet;  }
    set { this._ExclusionSet = value; }
  }

  public int Minimum
  {
    get { return this._MinSize; }
    set	
    { 
      this._MinSize = value;
      if ( PasswordGenerator.DefaultMinimum > this._MinSize )
      {
        this._MinSize = PasswordGenerator.DefaultMinimum;
      }
    }
  }

  public int Maximum
  {
    get { return this._MaxSize; }
    set	
    { 
      this._MaxSize = value;
      if ( this._MinSize >= this._MaxSize )
      {
        this._MaxSize = PasswordGenerator.DefaultMaximum;
      }
    }
  }

  public bool ExcludeSymbols
  {
    get { return this._HasSymbols; }
    set	{ this._HasSymbols = value;}
  }

  public bool RepeatCharacters
  {
    get { return this._HasRepeating; }
    set	{ this._HasRepeating = value;}
  }

  public bool ConsecutiveCharacters
  {
    get { return this._HasConsecutive; }
    set	{ this._HasConsecutive = value;}
  }
  #endregion
  #region "Constructors"
  public PasswordGenerator() 
  {
    this.Minimum               = DefaultMinimum;
    this.Maximum               = DefaultMaximum;
    this.ConsecutiveCharacters = false;
    this.RepeatCharacters      = true;
    this.ExcludeSymbols        = false;
    this.Exclusions            = null;

    _Rng = new RNGCryptoServiceProvider();
  }		
  #endregion
		
  #region "Internal methods"
  protected int GetCryptographicRandomNumber(int lBound, int uBound)
  {   
    // Assumes lBound >= 0 && lBound < uBound
    // returns an int >= lBound and < uBound
    uint urndnum;   
    byte[] rndnum = new Byte[4];   
    if (lBound == uBound-1)  
    {
      // test for degenerate case where only lBound can be returned
      return lBound;
    }
                                                              
    uint xcludeRndBase = (uint.MaxValue -
      (uint.MaxValue%(uint)(uBound-lBound)));   
            
    do 
    {      
      _Rng.GetBytes(rndnum);      
      urndnum = System.BitConverter.ToUInt32(rndnum,0);      
    } while (urndnum >= xcludeRndBase);   
            
    return (int)(urndnum % (uBound-lBound)) + lBound;
  }

  protected char GetRandomCharacter()
  {            
    int upperBound = _PwdCharArray.GetUpperBound(0);

    if ( true == this.ExcludeSymbols )
    {
      upperBound = PasswordGenerator.UBoundDigit;
    }

    int randomCharPosition = GetCryptographicRandomNumber(
      _PwdCharArray.GetLowerBound(0), upperBound);

    char randomChar = _PwdCharArray[randomCharPosition];

    return randomChar;
  }
  #endregion
  #region "Generate Function"
  public string Generate()
  {
    // Pick random length between minimum and maximum   
    int pwdLength = GetCryptographicRandomNumber(this.Minimum,
      this.Maximum);

    StringBuilder pwdBuffer = new StringBuilder();
    pwdBuffer.Capacity = this.Maximum;

    // Generate random characters
    char lastCharacter, nextCharacter;

    // Initial dummy character flag
    lastCharacter = nextCharacter = '\n';

    for ( int i = 0; i < pwdLength; i++ )
    {
      nextCharacter = GetRandomCharacter();

      if ( false == this.ConsecutiveCharacters )
      {
        while ( lastCharacter == nextCharacter )
        {
          nextCharacter = GetRandomCharacter();
        }
      }

      if ( false == this.RepeatCharacters )
      {
        string temp = pwdBuffer.ToString();
        int duplicateIndex = temp.IndexOf(nextCharacter);
        while ( -1 != duplicateIndex )
        {
          nextCharacter = GetRandomCharacter();
          duplicateIndex = temp.IndexOf(nextCharacter);
        }
      }

      if ( ( null != this.Exclusions ) )
      {
        while ( -1 != this.Exclusions.IndexOf(nextCharacter) )
        {
          nextCharacter = GetRandomCharacter();
        }
      }

      pwdBuffer.Append(nextCharacter);
      lastCharacter = nextCharacter;
    }

    if ( null != pwdBuffer )
    {
      return pwdBuffer.ToString();
    }
    else
    {
      return String.Empty;
    }	
  }
  #endregion
}
public class PasswordAnalyzer
{
  public static int GetBitSize(string password)
  {
    int charSet = 0;
    charSet = getCharSetUsed(password);

    /*This comes from the math involved in addressing the randomness of the 
     English language, as found in the research paper from Claude Shannon 
     titled "A Mathematical Theory of Computation" way back in 1948. 
     Basically the effective bit strength of a random password is 
     log2(n^m), where n is the pool size of valid characters and 
     m is the length of the password. The above calculation does just that!*/
    double result = Math.Log( Math.Pow( charSet, password.Length ) ) / Math.Log( 2 );

    return Convert.ToInt32(result);
  }

  private static int getCharSetUsed( string pass )
  {
    int ret = 0;

    if( containsNumbers( pass ) )
    {
      ret += 10;
    }

    if( containsLowerCaseChars( pass ) )
    {
      ret += 26;
    }

    if( containsUpperCaseChars( pass ) )
    {
      ret += 26;
    }

    if( containsPunctuation( pass ) )
    {
      ret += 31;
    }

    return ret;
  }

  private static bool containsNumbers( string str )
  {
    Regex pattern = new Regex( @"[\d]" );
    return pattern.IsMatch( str );
  }

  private static bool containsLowerCaseChars( string str )
  {
    Regex pattern = new Regex( "[a-z]" );
    return pattern.IsMatch( str );
  }

  private static bool containsUpperCaseChars( string str )
  {
    Regex pattern = new Regex( "[A-Z]" );
    return pattern.IsMatch( str );
  }

  private static bool containsPunctuation( string str )
  {
    // regular expression include _ as a valid char for alphanumeric.. 
    // so we need to explicity state that its considered punctuation.
    Regex pattern = new Regex( @"[\W|_]" );
    return pattern.IsMatch( str );
  }
}
