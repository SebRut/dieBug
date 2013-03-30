<?php
if(file_exists("./".$_GET['id']."/") == false) {
	echo "<h1>I'm 404 and I know it!</h1>";
	die();
}
$xml = simplexml_load_file('./'.$_GET['id'].'/screenshot.xml', 'SimpleXMLElement', LIBXML_NOCDATA);

$att = $xml->attributes();

$datei = "../".$_GET['id']."/".htmlentities($att["filename"]);
$description = $xml->content[0];
$datum = date("d.m.Y",(string) $xml->attributes()->timestamp);
$hexcolor = $xml->attributes()->hexcolor;
$comphexcolor = $xml->attributes()->comphexcolor;

$lcolor = $hexcolor;
$l2color = HexToRGB($hexcolor);
$cav = (($l2color["r"]+$l2color["g"]+$l2color["b"])/3);
//$lcolor = HexToRGB($hexcolor);
//$cav = (($lcolor["r"]+$lcolor["g"]+$lcolor["b"])/3);
//$lcolor["r"] = ((($cav-$lcolor["r"])/2)+$lcolor["r"])+(230-$cav);
//$lcolor["g"] = ((($cav-$lcolor["g"])/2)+$lcolor["g"])+(230-$cav);
//$lcolor["b"] = ((($cav-$lcolor["b"])/2)+$lcolor["b"])+(230-$cav);
//
//$lcolor = RGBToHex($lcolor["r"],$lcolor["g"],$lcolor["b"]);
?>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
	<head>
		<title>dieBug #<?php echo $_GET['id'] ?></title>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
        <link href='http://fonts.googleapis.com/css?family=Lato:300,400,700,900' rel='stylesheet' type='text/css'>
		<link href="../format.css" rel="stylesheet" type="text/css">
        <script src="../jquery-1.9.1.min.js" type="text/javascript"></script>
        <script src="../jquery.easing.1.3.js" type="text/javascript"></script>
        <script src="../jquery.blockUI.js" type="text/javascript"></script>
        <script src="../script.js" type="text/javascript"></script>
	</head>
	<body>
    	<div id="sidebar" style="background-color:<?php echo $lcolor; ?>">
        	<div id="sidebarcontainer" style="color:<?php echo $cav > 200 ? "#000000" : "#FFFFFF"; ?>;">
            	<div align="right">
                	<input type="button" value="Großes Bild laden" onClick="$('#srcimg').attr('src','<?php echo $datei; ?>.big.png'); $(this).slideUp();"
                    style="width:100%; height:30px; background-color:<?php echo $lcolor; ?>; border:solid 1px <?php echo $cav > 200 ? "#000000" : "#FFFFFF"; ?>; color:<?php echo $cav > 200 ? "#000000" : "#FFFFFF"; ?>"/>
                </div>
            	<img id="thumbimg" src="<?php echo $datei; ?>.thumb.png" style="border:solid 5px <?php echo $comphexcolor; ?>;"/><br/>
                <div id="date"><?php echo $datum; ?></div>
                <div id="description">
                	<div style="border-bottom:solid 1px <?php echo $cav > 200 ? "#000000" : "#FFFFFF"; ?>; width:100%; font-size:18px; margin-bottom:-15px">BESCHREIBUNG</div><br/>
                	<?php echo nl2br(htmlentities($description, ENT_NOQUOTES, 'UTF-8')); ?>
                </div>
            </div>
        </div>
        <div id="arrow" style="border-color: transparent <?php echo $lcolor; ?> transparent transparent;">
        	
        </div>
    	<div id="sidebarblocker">
        	
        </div>
		<div id="image">
        	<img id="srcimg" src="<?php echo $datei; ?>.small.png"/>
        </div>
        <div id="vignette"></div>
        <div id="background" style="background-color: <?php echo $hexcolor; ?>"></div>
	</body>
</html>
<?php
	function HexToRGB($hex) {
		$hex = str_replace("#", "", $hex);
		$color = array();
 
		if(strlen($hex) == 3) {
			$color['r'] = hexdec(substr($hex, 0, 1) . $r);
			$color['g'] = hexdec(substr($hex, 1, 1) . $g);
			$color['b'] = hexdec(substr($hex, 2, 1) . $b);
		}
		else if(strlen($hex) == 6) {
			$color['r'] = hexdec(substr($hex, 0, 2));
			$color['g'] = hexdec(substr($hex, 2, 2));
			$color['b'] = hexdec(substr($hex, 4, 2));
		}
 
		return $color;
	}
	
	function mx($val) {
		return $val > 255 ? 255 : $val;
	}
	function RGBToHex($r, $g, $b) {
		$hex = "#";
		$hex.= str_pad(dechex($r), 2, "0", STR_PAD_LEFT);
		$hex.= str_pad(dechex($g), 2, "0", STR_PAD_LEFT);
		$hex.= str_pad(dechex($b), 2, "0", STR_PAD_LEFT);
 
		return $hex;
	}
?>
?>