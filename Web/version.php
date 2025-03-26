<?php
header("Content-type:application/json");

// Create our response...
$response = array(
    'Version' => "1.01",
    'SourceF' => "https://lawful.swordofmoonlight.com/V101_dev.zip"
);

// Conver to json and echo out
$response_json = json_encode($response);
echo $response_json;

?>