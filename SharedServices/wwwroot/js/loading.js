function startLoading($object, message) {
    $object.jloader();
    $object.jloader('show', message);
}

function stopLoading($object) {
    $object.jloader('hide');
}