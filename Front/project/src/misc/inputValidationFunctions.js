export const emailValidation = (value) => {
  if (value === 'admin') {
    return { isValid: true, errorMessage: '' };
  }
  if (!value.includes('@')) {
    return { isValid: false, errorMessage: 'Email adress must inlcude "@"!' };
  } else if (value.length < 1) {
    return { isValid: false, errorMessage: 'Field can not be left empty!' };
  }

  return { isValid: true, errorMessage: '' };
};

export const passwordValidation = (value) => {
  if (value === 'admin') {
    return { isValid: true, errorMessage: '' };
  }
  if (value.length < 7) {
    return {
      isValid: false,
      errorMessage: 'Password must be at least 7 characters long!',
    };
  }

  return { isValid: true, errorMessage: '' };
};

export const regularFieldValidation = (value) => {
  if (value.length < 1) {
    return { isValid: false, errorMessage: 'Field can not be left empty!' };
  }

  return { isValid: true, errorMessage: '' };
};
