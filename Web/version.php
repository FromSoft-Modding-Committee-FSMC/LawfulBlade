<?php
header("Content-type:application/json");

// Create our response...
$response = array(
    'Version' => "0.00",
    'SourceF' => "https://lawful.swordofmoonlight.com"
);

// Conver to json and echo out
$response_json = json_encode($response);
echo $response_json;

?>