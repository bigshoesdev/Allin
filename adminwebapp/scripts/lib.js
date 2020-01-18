//#####################################################################################//
/*
lib.js 함수 목록.

- is_pass(pass,pass2)
  pass , pass2 값을 비교하여 자료가 일치하는가를 판별한다. (※ 공백[space]만으로 입력 불가)

- trim(str)
  str의 공백[space]를 모두 없에준다.

- code_chk(f,msg,code)
  f의 자료형태가 code 형식의 값만을 받을수 있게 한다.
  msg 는 에러문의 메세지 이다.
  ex) code_chk(fn.code,"번호는 [숫자]만 사용가능 합니다.","1234567890")
 
- is_number(str)
  str의 입력값은 [숫자,-]만 입력 돼었는가를 판별한다.

- is_tel(tel)
  tel의 값이 핸드폰 , 일반전화번호 인가를 구분하며 해당 메세지를 출력한다.
  ex) is_tel("011-1234-5678")

- is_cellphone(tel)
  tel의 값이 핸드폰번호 형식의 값인가를 판별한다.

- is_phone(tel)
  tel의 값이 일반전화번호 형식의 값인가를 판별한다.

- tel_phone(f)
  f의 값에 핸드폰번호 국번값을 넣어준다 (※ f는 select 형식이여야 한다.)

- tel_local(f)
  f의 값에 일반전화번호 국번값을 넣어준다 (※ f는 select 형식이여야 한다.)

- fun_msg(src,msg)
  src의 값유무를 판단한다. 있을경우 true 없을경우 false를 반환한다.
  ex) fun_msg(fn.code,'CODE') => CODE(을)를 입력하세요. 의메세지가 뜬다.

- auto_focus(f,len,next)
  f값의 길이가 len과 같을경우 next 포커스로 이동 한다.

- len_chk(f,min,max,msg)
  f의 값이 최소(min) 최대(max) 사이의 값만을 받을수 있게 한다.
  min 의 값이 1 또는 0 이면 입력값 유무만 판단한다.
  min 과 max 같을때 min(max) 값이상을 입력받았는지 판별한다.

- indata_chk(f,astr,min,max,msg)
  f의 값이 astr 형태의 값으로 min 이상 max 이하의 자료인가를 판별한다.
  cf)code_chk() 비슷한 형식.

- id_chk(f,min,max,msg)
  f 의값이 통신상의 ID 값으로 충분한가를 판별한다.
  ※ 처음은 영문으로 시작한다.

- mail_chk(f)
  f의 형식이 전자메일(e-mail) 형식값인가를 판별한다.

- function jumin_chk(f)
  주민등록번호 값인가를 반환한다. 맞으면 true, 실릴경우 false 반환

- is_GraphicExt(str)
  첨부자료의 확장자를 이미지형 확장자인지 판별한다.

*/
//#####################################################################################//


// 비밀번호 비교 루틴
function is_pass(pass, pass2, min, max)
{
	if(!len_chk(pass, min, max, "비밀번호")) return false;
	if(!len_chk(pass2, min, max, "비밀번호확인")) return false;
  /*
	if(trim(pass.value) <= min) {
		alert("스페이스(공백) 만으로 암호를 만들지 못합니다.");
		pass.focus();
		return false;
	}
  */
	if(pass.value != pass2.value) 
	{
		alert("비밀번호가 서로 다르게 입력 돼었습니다!!");
		pass2.value = "";
		pass.value = "";
		pass.focus();
		return false;
	}
	return true;
}

// trim 함수
function trim(str) 
{
	var count = str.length;
	var len = count;                
	var st = 0;
                
	while ((st < len) && (str.charAt(st) <= ' ')) {
		st++;
	}
	while ((st < len) && (str.charAt(len - 1) <= ' ')) {
		len--;
	}                
	return ((st > 0) || (len < count)) ? str.substring(st, len) : str ;   
}

// code형식 입력 판단 함수.
function code_chk(f,msg,code)
{
	var str = f.value;
	for (i=0; i < str.length; i++)
	{
		if(code.indexOf(str.substring(i,i+1))<0) 
		{
			alert(msg);
			f.select();
			f.focus();
			return false;
		}
	}
	return true;
}

// [숫자,-] 입력 판단 함수
function is_number(str)
{
	var num = str.value;
	if (num.search(/^[\d-]*$/) < 0)
	{
		alert("숫자와 '-' 기호만 사용가능합니다.");
		str.focus();
		return false;
	}
	return true;
}

function is_onlynumber(str)
{
	var num = str.value;
	if (num.search(/^[\d]*$/) < 0)
	{
		alert("숫자만 사용가능합니다.");
		str.focus();
		return false;
	}
	return true;
}

//전화번호 종류확인
function is_tel(tel)
{
	var tel_num;
	tel_num = tel.value;

    if (tel_num.length == 0)
    {
      alert("핸드폰이나 전화번호 중 하나를 입력하세요.");
	  tel.focus();
      return false;
    }

	if(tel_num.substr(0,2) == "01") 
	{
		return is_cellphone(tel)
	}
	else
	{
		return is_phone(tel)
	}
}

// 핸드폰 입력 양식 확인
function is_cellphone(tel)
{
	var head, tel_num;
	if(! code_chk(tel,"핸드폰 번호는 [숫자,-]만 사용가능 합니다.","1234567890-")) return false;
	tel_num = tel.value;
	for(var i=0;i<tel_num.length;i++)
	{
		tel_num = tel_num.replace("-","");
	}
	if(tel_num.search("-") <0)
	{
		if(tel_num.length == 10)
		{
			tel_num = tel_num.substr(0,3) + "-" + tel_num.substr(3,3) + "-" + tel_num.substr(6,4);
		}
		else if(tel_num.length == 11)
		{
			tel_num = tel_num.substr(0,3) + "-" + tel_num.substr(3,4) + "-" + tel_num.substr(7,4);
		}
	}
	if (tel_num.length > 0)
	{
		if (tel_num.search(/^01\d-\d{3,4}-\d{4}$/) < 0)
		{
			alert("핸드폰 번호를 잘못 입력하셨습니다.\n\n올바른 예 : 01X-XXX-XXXX");
			tel.focus();
			return false;
		}
		head = tel_num.split("-");
		if(head[0] != "010" && head[0] != "011" && head[0] != "016" && head[0] != "017" && head[0] != "018" && head[0] != "019")
		{
			alert("핸드폰 번호는 010, 011, 016, 017 , 018, 019 만 사용 가능합니다.");
			tel.focus();
			return false;
		}
	}
	tel.value = tel_num;
	return true;
}

// 일반전화번호 입력 양식 확인
function is_phone(tel)
{
	var head, tel_num;
	if(!code_chk(tel,"전화번호는 [숫자,-]만 사용가능 합니다.","1234567890-")) return false;
	tel_num = tel.value;
	for(var i=0;i<tel_num.length;i++)
	{
		tel_num = tel_num.replace("-","");
	}
	if(tel_num.search("-") <0)
	{
		if(tel_num.substr(0,2) == "02")
		{
			if(tel_num.length == 9)
			{
				tel_num = tel_num.substr(0,2)+"-"+tel_num.substr(2,3)+"-"+tel_num.substr(5,4);
			}
			else if (tel_num.length == 10)
			{
				tel_num = tel_num.substr(0,2)+"-"+tel_num.substr(2,4)+"-"+tel_num.substr(6,4);
			}
			else
			{
				tel_num = tel_num.substr(0,2)+"-"+tel_num.substr(2,4)+"-"+tel_num.substr(6,4); //tel_num.length
			}
		}
		else if(tel_num.length > 0)
		{
			if(tel_num.length < 10)
			{
				alert("전화번호를 잘못 입력하셨습니다.");
				tel.focus();
				return false;
			}
			if(tel_num.length == 10)
			{
				tel_num = tel_num.substr(0,3) + "-" + tel_num.substr(3,3) + "-" + tel_num.substr(6,4);
			}
			else if(tel_num.length == 11)
			{
				tel_num = tel_num.substr(0,3) + "-" + tel_num.substr(3,4) + "-" + tel_num.substr(7,4);
			}
			else
			{
				tel_num = tel_num.substr(0,3)+"-"+tel_num.substr(3,4)+"-"+tel_num.substr(7,4); //tel_num.length
			}
		}
	}
	if (tel_num.length > 0)
	{
		head = tel_num.split("-");
		if (head[0] == "02" && tel_num.search(/^02-\d{3,4}-\d{4}$/) < 0)
		{
			alert("전화번호를 잘못 입력하셨습니다.\n\n올바른 예 : 02-XXX-XXXX , 02-XXXX-XXXX");
			tel.focus();
			return false;
		}
		else if (head[0].length == 3 && tel_num.search(/^0\d{2}-\d{3,4}-\d{4}$/) < 0)
		{
			alert("전화번호를 잘못 입력하셨습니다.\n\n올바른 예 : 0XX-XXX-XXXX , 0XX-XXXX-XXXX");
			tel.focus();
			return false;
		}
	}
	tel.value = tel_num;

	//if(typeof(head[3]) == 'undefined')
	if(tel_num.search(/^0\d{1,2}-\d{3,4}-\d{4}$/) < 0)
	{
		alert("전화번호를 잘못 입력하셨습니다.\n\n올바른 예 : 0XX-XXX-XXXX , 0XX-XXXX-XXXX");
		tel.focus();
		return false;
	}
	return true;
}

//핸드폰 국번 펼침목록부
function tel_phone(f)
{
	f.options.length=6;
	f[0].text='010';
	f[1].text='011';
	f[2].text='016';
	f[3].text='017';
	f[4].text='018';
	f[5].text='019';
	f[0].value='010';
	f[1].value='011';
	f[2].value='016';
	f[3].value='017';
	f[4].value='018';
	f[5].value='019';
}

//전화번호 지방국번 펼침목록부
function tel_local(f) 
{
	f.options.length=16;
	f[0].text=' 02[서울]';
	f[1].text='031[경기]';
	f[2].text='032[인천]';
	f[3].text='033[강원]';
	f[4].text='041[충남]';
	f[5].text='042[대전]';
	f[6].text='043[충북]';
	f[7].text='051[부산]';
	f[8].text='052[울산]';
	f[9].text='053[대구]';
	f[10].text='054[경북]';
	f[11].text='055[경남]';
	f[12].text='061[전남]';
	f[13].text='062[광주]';
	f[14].text='063[전북]';
	f[15].text='064[제주]';
	f[0].value='02';
	f[1].value='031';
	f[2].value='032';
	f[3].value='033';
	f[4].value='041';
	f[5].value='042';
	f[6].value='043';
	f[7].value='051';
	f[8].value='052';
	f[9].value='053';
	f[10].value='054';
	f[11].value='055';
	f[12].value='061';
	f[13].value='062';
	f[14].value='063';
	f[15].value='064';
}

//입력값이 없을경우 메세지 출력 함수
function fun_msg(src,msg)
{
	if(! src.value)
	{
		alert(msg + '(을)를 입력해 주세요!!');
		src.focus();
		return false;
	}
	return true;
}

//길이만큼 입력후 다음 커스로 자동이동
function auto_focus(f,len,next) 
{
	var str = f.value;
	if (str.length == len) 
	{
		next.focus();
	}
}

//입력 데이타의 길이 측정, 제한하는 함수
function len_chk(f,min,max,msg)	
{
	var str = f.value;
	if(str.length < min || str.length > max ) 
	{
		if (min == 1 || min == 0) 
		{
			if(str.length == 0) 
			{
				alert(msg + "을(를) 입력하지 않으셨습니다");
			}
			else 
			{
				alert(msg + "은(는) " + max + " 자를 넘을 수 없습니다");
			}
		} 
		else if (min == max) 
		{
			alert(msg + "은(는) " + min + " 자를 입력해야 합니다");
		} 
		else 
		{
			alert(msg + "은(는) " + min + " 자이상 " + max + " 이하여야 합니다");
		}
		f.focus();
		return false;
	}
	return true;
}

//입력 데이타의 내부 입력값 제한 함수
function indata_chk(f,astr,min,max,msg) 
{
	var str = f.value;
	if(!len_chk(f,min,max,msg)) 
	{
		return false;
	}
	if (astr.length > 1) 
	{
		for (i=0; i < str.length; i++)
		{
			if(astr.indexOf(str.substring(i,i+1))<0) 
			{
				alert(msg + "에 허용할 수 없는 문자가 입력되었습니다.");
				f.focus();
				return false;
			}
		}
	}
	return true;
}

//아이디 입력양식
function id_chk(f,min,max,msg) 
{
	var str = f.value;
	var chk = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_';
	var fchk = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';

	if(!len_chk(f,min,max,msg)) 
	{
		return false;
	}
	if(fchk.indexOf(str.substring(0,1))<0) 
	{
		alert("ID의 첫문자는 반드시 영문자 이어야합니다.");
		f.value = "";
		f.focus();
		return false;
	}
	for (i=1; i < str.length; i++)
	{
		if(chk.indexOf(str.substring(i,i+1))<0) 
		{
			alert("ID는 반드시 '영문자','숫자','_' 만 사용가능 합니다.");
			f.value = "";
			f.focus();
			return false;
		}
	}
	return true;
}

//닉네임 입력양식
function nicname_chk(f,min,max,msg) 
{
	var str = f.value;
	if(!len_chk(f,min,max,msg)) 
	{
		return false;
	}
  var err_str = "";
  for (i=0; i < str.length; i++) {
    char_ASCII = str.charCodeAt(i);
    if ((char_ASCII>=33 && char_ASCII<=47) || (char_ASCII>=58 && char_ASCII<=64) || (char_ASCII>=91 && char_ASCII<=96) || (char_ASCII>=123 && char_ASCII<=126)) err_str += str.substring(i,i+1) + " ";
  }
  if(err_str == "") return true;
  else {
    alert("사용할수 없는 특수 문자 \n\n" + err_str + " 가 포함 되었습니다.");
    return false;
  }
}

//email 입력양식
function mail_chk(f) 
{
 	var str = f.value;
	emailEx1 = /[^@]+@[A-Za-z0-9_\-]+\.[A-Za-z]+/;
	emailEx2 = /[^@]+@[A-Za-z0-9_\-]+\.[A-Za-z0-9_\-]+\.[A-Za-z]+/;
	emailEx3 = /[^@]+@[A-Za-z0-9_\-]+\.[A-Za-z0-9_\-]+\.[A-Za-z0-9_\-]+\.[A-Za-z]+/;
	//emailEx4 = /[A-Za-z0-9_\-]+\.[A-Za-z0-9_\-]+@[A-Za-z0-9_\-]+\.[A-Za-z0-9_\-]+\.[A-Za-z0-9_\-]+\.[A-Za-z]+/;
	if(emailEx1.test(str)) 
	{ 
		return true; 
	}
	if(emailEx2.test(str))
	{ 
		return true; 
	}
	if(emailEx3.test(str))
	{ 
		return true; 
	}
	//if(emailEx4.test(str)) { return true; }
	alert("E-Mail주소형식이 올바르지 않습니다");
	f.focus();
	return false;
}


//주민등록번호 체크
function jumin_chk(f) 
{
	if(! is_number(f)) return false;
	//if(! code_chk(f,"주민등록번호는 [숫자,-]만 사용가능 합니다.","1234567890-")) return false;
	var str = f.value;
	var sum=0;
	var chk=0;

	for(var i=0;i<str.length;i++)
	{
		str = str.replace("-","");
	}
	if(! str.search("-") < 0 || str.length != 13) 
	{
		alert('주민등록번호 13자리를 바르게 입력해 주세요.');
		f.focus();
		return false;
	}

	for(i=0;i<12;i++) 
	{
		if(i<8)
		{
			sum+=parseInt(str.charAt(i))*(i+2);
		}
		if(i>7)
		{
			sum+=parseInt(str.charAt(i))*(i-6);
		}
	}
	chk = (sum%11) + parseInt(str.charAt(12));
	if(!(chk == 1 || chk == 11)) 
	{
		alert("주민등록 번호가 올바르지 않습니다.");
		f.focus();
		return false;
	}

	f.value = str.substr(0,6) + "-" + str.substr(6,7);
	return true;
}

function jumin_chk2(ssn1, ssn2)
{
	if(! code_chk(ssn1, "주민등록번호 앞자리는 [숫자]만 사용가능 합니다.", "1234567890") || ! len_chk(ssn1, 6, 6, "주민등록번호 앞자리")) return false;
	if(! code_chk(ssn2, "주민등록번호 뒷자리는 [숫자]만 사용가능 합니다.", "1234567890") || ! len_chk(ssn2, 7, 7, "주민등록번호 뒷자리")) return false;
  var ssn = ssn1.value + ssn2.value;
	var sum = 0;
	var chk = 0;

	for(i=0;i<12;i++) 
	{
		if(i < 8) {	sum += parseInt(ssn.charAt(i)) * (i + 2); }
		if(i > 7) { sum += parseInt(ssn.charAt(i)) * (i - 6); }
	}

	chk = (sum%11) + parseInt(ssn.charAt(12));
	if(!(chk == 1 || chk == 11)) 
	{
		alert("주민등록 번호가 올바르지 않습니다.");
		ssn1.focus();
		return false;
	}

  return true;
}

function is_GraphicExt(str)
{
	var g = str.value;
	var dot = 0;
	var ext = "";
 
	dot = g.lastIndexOf(".");
	ext = g.substr(dot+1,g.length ).toLowerCase();
	if(ext == "png" || ext == "bmp" || ext == "gif" || ext == "jpg" || ext == "jpeg") return;
	else 
	{
		alert("이미지 파일만 업로드가 가능합니다.");
		return false;
	}
}

//**********홈페이지 주소 형식 체크
function is_URL(url)
{
  if(! isURL(url))
  {
  	alert("홈페이지 주소형식이 아닙니다");
	  url.focus();
  	return false;
  }
  else
  {
    return true;
  }
}

function URL_chk(f)
{
	var g = f.value;
	if ( g.length ==7  && g.substr( 0, 7 ) == "http://" )
	{
		return true;
	} 
	else if ( g.length > 7 && g.indexOf(".") > 7 && g.lastIndexOf(".") != g.length-1 )
	{
		return true;
	} 
	alert("홈페이지 주소형식이 아닙니다");
	f.focus();
	return false;
}

function isURL(url)
{
  var argvalue = url.value.toLowerCase();
	if (argvalue.indexOf(" ") != -1) return false;
	else if (argvalue.search(/^http:/) < 0 && argvalue.search(/^https:/) < 0)
  {
    url.value = "http://" + argvalue;
  }
  else if ((argvalue.search(/^http:/) >= 0 && argvalue.length > 7) || (argvalue.search(/^https:/) >= 0 && argvalue.length > 8))
  {
    var url_head, url_string;
    if(argvalue.substr(0,5) == "http:")
    {
      url_head = "http://";
      url_string = argvalue.substr(5,argvalue.length);
    }
    else
    {
      url_head = "https://";
      url_string = argvalue.substr(6,argvalue.length);
    }

    var count = url_string.split("/");
    url_string = "";
    for(var i = 0;i < count.length;i++)
    {
      if(trim(count[i]) != "")
      {
        url_string += trim(count[i]) + "/";
      }
    }
    url.value = url_head + url_string.substr(0,url_string.length - 1);
  }

	argvalue = argvalue.substring(7, argvalue.length);
	if (argvalue.indexOf(".") == -1) return false;
	else if (argvalue.indexOf(".") == 0) return false;
	else if (argvalue.charAt(argvalue.length - 1) == ".") return false;

	if (argvalue.indexOf("/") != -1) 
	{
		argvalue = argvalue.substring(0, argvalue.indexOf("/"));
		if (argvalue.charAt(argvalue.length - 1) == ".")
		{
			return false;
		}
	}

	if (argvalue.indexOf(":") != -1) 
	{
		if (argvalue.indexOf(":") == (argvalue.length - 1)) return false;
		else if (argvalue.charAt(argvalue.indexOf(":") + 1) == ".") return false;
		argvalue = argvalue.substring(0, argvalue.indexOf(":"));
		if (argvalue.charAt(argvalue.length - 1) == ".") return false;
	}

	return true;
}