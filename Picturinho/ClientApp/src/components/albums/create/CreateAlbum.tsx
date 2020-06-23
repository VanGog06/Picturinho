import React, { useCallback, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { Dispatch } from 'redux';

import { AlbumModel } from '../../../models/album/AlbumModel';
import { UserModel } from '../../../models/user/UserModel';
import { AlbumService } from '../../../services/AlbumService';
import { AlertActions, alertError } from '../../../store/alert/AlertActions';

export const CreateAlbum: React.FC = (): JSX.Element => {
  const dispatch = useDispatch<Dispatch<AlertActions>>();
  const history = useHistory<History>();

  const user: UserModel | undefined = useSelector(
    (state: any) => state.authentication?.user
  );
  const [submitted, setSubmitted] = useState<boolean>(false);
  const [inputs, setInputs] = useState<AlbumModel>({
    id: 0,
    description: "",
    name: "",
    userId: user ? user.id : 0,
  });

  const handleInputChange = useCallback(
    (event: React.ChangeEvent<HTMLInputElement>): void => {
      const { name, value } = event.target;
      setInputs((inputs: AlbumModel) => ({ ...inputs, [name]: value }));
    },
    [setInputs]
  );

  const handleTextAreaChange = useCallback(
    (event: React.ChangeEvent<HTMLTextAreaElement>): void => {
      const { name, value } = event.target;
      setInputs((inputs: AlbumModel) => ({ ...inputs, [name]: value }));
    },
    [setInputs]
  );

  const handleSubmit = useCallback(
    async (event: React.FormEvent<HTMLFormElement>) => {
      setSubmitted(true);

      try {
        event.preventDefault();

        if (inputs.name) {
          await AlbumService.createAlbum(inputs);
          history.push("/albums");
        } else {
          setSubmitted(false);
        }
      } catch (err) {
        setSubmitted(false);
        dispatch(alertError(err.message));
      }
    },
    [inputs, setSubmitted, dispatch, history]
  );

  return (
    <div className="col-lg-8 offset-lg-2">
      <h2>Create album</h2>
      <form name="form" onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="name">Name</label>
          <input
            type="text"
            name="name"
            value={inputs.name}
            required
            onChange={handleInputChange}
            className={
              "form-control" + (submitted && !inputs.name ? " is-invalid" : "")
            }
          />
          {submitted && !inputs.name && (
            <div className="invalid-feedback">Name is required</div>
          )}
        </div>

        <div className="form-group">
          <label htmlFor="description">Description</label>
          <textarea
            name="description"
            value={inputs.description}
            onChange={handleTextAreaChange}
            className="form-control"
          ></textarea>
        </div>

        <div className="form-group">
          <button className="btn btn-primary">Create</button>
        </div>
      </form>
    </div>
  );
};
